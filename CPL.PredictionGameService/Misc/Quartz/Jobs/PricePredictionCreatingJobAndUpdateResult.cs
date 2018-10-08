using CPL.Common.Enums;
using CPL.Common.Misc;
using CPL.Core.Services;
using CPL.Domain;
using CPL.Infrastructure.Repositories;
using Microsoft.Extensions.PlatformAbstractions;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPL.PredictionGameService.Misc.Quartz.Jobs
{
    internal class PricePredictionCreatingJobAndUpdateResult : IJob
    {
        public string FileName { get; set; }
        private static int PricePredictionBettingIntervalInHour;        // 8h
        private static int PricePredictionHoldingIntervalInHour;        // 1h
        private static int PricePredictionCompareIntervalInMinute;      // 15m
        private static int PricePredictionGameIntervalInHour;           // 24h

        public PricePredictionCreatingJobAndUpdateResult()
        {
            FileName = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "log.txt");
        }

        public Task Execute(IJobExecutionContext context)
        {
            Utils.FileAppendThreadSafe(FileName, string.Format("{0}Execute PricePredictionCreatingJob {1}: {2}", Environment.NewLine, DateTime.Now, Environment.NewLine));

            JobDataMap dataMap = context.JobDetail.JobDataMap;
            Resolver resolver = (Resolver)dataMap["Resolver"];

            PricePredictionBettingIntervalInHour = int.Parse(resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.PricePredictionBettingIntervalInHour).Value);
            PricePredictionHoldingIntervalInHour = int.Parse(resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.PricePredictionHoldingIntervalInHour).Value);
            PricePredictionCompareIntervalInMinute = int.Parse(resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.PricePredictionCompareIntervalInMinute).Value);
            PricePredictionGameIntervalInHour = int.Parse(resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.PricePredictionGameIntervalInHour).Value);

            DateTime localDateTime = context.FireTimeUtc.LocalDateTime;

            // Create new price prediction game
            DoCreatePricePrediction(ref resolver, localDateTime);

            // Update result game
            int pricePredictionId = DoGetBTCPrice(ref resolver, localDateTime);
            if (pricePredictionId > 0)
                DoUpdateWinner(ref resolver, pricePredictionId);

            return Task.FromResult(0);
        }

        public void DoCreatePricePrediction(ref Resolver resolver, DateTime localDateTime)
        {
            Utils.FileAppendThreadSafe(FileName, string.Format("1. DoCreatePricePrediction--OpenBettingTime is {0}: {1}{2}", localDateTime, DateTime.Now, Environment.NewLine));

            var newPricePredictionRecord = new PricePrediction
            {
                Coinbase = EnumCurrencyPair.BTCUSDT.ToString(),
                OpenBettingTime = localDateTime,
                CloseBettingTime = localDateTime.AddHours(PricePredictionGameIntervalInHour - PricePredictionHoldingIntervalInHour).AddMinutes(-PricePredictionCompareIntervalInMinute),
                ToBeComparedTime = localDateTime.AddHours(PricePredictionGameIntervalInHour).AddMinutes(-PricePredictionCompareIntervalInMinute),
                ResultTime = localDateTime.AddHours(PricePredictionGameIntervalInHour),
            };

            resolver.PricePredictionService.Insert(newPricePredictionRecord);

            var LangIds = resolver.LangService.Queryable().Select(x => x.Id).ToList();
            var title = newPricePredictionRecord.CloseBettingTime.ToString("HH:mm") == "00:00" ? "24:00" : newPricePredictionRecord.CloseBettingTime.ToString("HH:mm");
            foreach (var id in LangIds)
            {
                resolver.PricePredictionDetailService.Insert(new PricePredictionDetail
                {
                    LangId = id,
                    Title = title, // title will be shown as named of tab in priceprediction screen
                    PricePredictionId = newPricePredictionRecord.Id,
                });

            }

            // udpate DB
            resolver.UnitOfWork.SaveChanges();
        }

        private int DoGetBTCPrice(ref Resolver resolver, DateTime resultTimeLocal)
        {
            Utils.FileAppendThreadSafe(FileName, string.Format("2. DoGetBTCPrice--OpenBettingTime is {0} at: {1}{2}", resultTimeLocal, DateTime.Now, Environment.NewLine));
            try
            {
                // result time and price
                var resultTime = ((DateTimeOffset)resultTimeLocal).ToUnixTimeSeconds();
                var resultPrize = resolver.BTCPriceService.Queryable().OrderByDescending(x => x.Time).FirstOrDefault(x => resultTime >= x.Time).Price;

                // the time to be compared time and price
                var toBeComparedTime = ((DateTimeOffset)resultTimeLocal.AddMinutes(-PricePredictionCompareIntervalInMinute)).ToUnixTimeSeconds();
                var toBeComparedPrice = resolver.BTCPriceService.Queryable().OrderByDescending(x => x.Time).FirstOrDefault(x => toBeComparedTime >= x.Time).Price;

                // update price prediction
                var pricePrediction = resolver.PricePredictionService.Queryable().FirstOrDefault(x => !x.ResultPrice.HasValue && !x.ToBeComparedPrice.HasValue && resultTimeLocal.ToString("dd-MM-yyyy HH:mm") == x.ResultTime.ToString("dd-MM-yyyy HH:mm"));
                pricePrediction.ResultPrice = resultPrize;
                pricePrediction.ToBeComparedPrice = toBeComparedPrice;
                pricePrediction.UpdatedDate = DateTime.Now;

                resolver.PricePredictionService.Update(pricePrediction);

                // save to DB
                resolver.UnitOfWork.SaveChanges();

                return pricePrediction.Id;
            }
            catch (Exception ex)
            {
                if (ex.InnerException?.Message != null)
                    Utils.FileAppendThreadSafe(FileName, string.Format("  + DoGetBTCPrice -- Exception {0} at {1}{2}", ex.InnerException.Message, DateTime.Now, Environment.NewLine));
                else
                    Utils.FileAppendThreadSafe(FileName, string.Format("  + DoGetBTCPrice -- Exception {0} at {1}{2}", ex.Message, DateTime.Now, Environment.NewLine));

                return 0;
            }
        }

        private void DoUpdateWinner(ref Resolver resolver, int pricePredictionId)
        {
            Utils.FileAppendThreadSafe(FileName, string.Format("3. DoUpdateWinner--PricePredictionId = {0} at: {1}{2}", pricePredictionId, DateTime.Now, Environment.NewLine));
            try
            {
                var pricePrediction = resolver.PricePredictionService
                    .Queryable()
                    .FirstOrDefault(x => x.Id == pricePredictionId);

                var pricePredictionHistories = resolver.PricePredictionHistoryService
                    .Query()
                    .Include(x => x.SysUser)
                    .Where(x => x.PricePredictionId == pricePredictionId);

                // result of game
                bool? gameResult = null; // Assumption: ResultPrice = ToBeComparedPrice
                if (pricePrediction.ResultPrice > pricePrediction.ToBeComparedPrice)
                {
                    gameResult = EnumPricePredictionStatus.UP.ToBoolean();
                }
                else if (pricePrediction.ResultPrice < pricePrediction.ToBeComparedPrice)
                {
                    gameResult = EnumPricePredictionStatus.DOWN.ToBoolean();
                }

                // calculate the prize
                decimal totalAmountOfLosers = 0.0m;
                decimal totalAmountToBeAwarded = 0.0m;
                decimal totalAmountOfWinners = 0.0m;

                if (gameResult.HasValue)
                {
                    // calculate the prize
                    totalAmountOfLosers = pricePredictionHistories.Where(x => x.Prediction != gameResult).Sum(x => x.Amount);
                    totalAmountToBeAwarded = totalAmountOfLosers * CPLConstant.PricePredictionTotalAwardPercentage; // Distribute 80% of the loser's BET quantity to the winners.
                    totalAmountOfWinners = pricePredictionHistories.Where(x => x.Prediction == gameResult).Sum(x => x.Amount);
                }

                // calculate the award
                foreach (var pricePredictionHistory in pricePredictionHistories)
                {
                    // refund the money for users
                    if (!gameResult.HasValue)
                    {
                        pricePredictionHistory.SysUser.TokenAmount += pricePredictionHistory.Amount;
                        pricePredictionHistory.Result = EnumGameResult.REFUND.ToString();
                        pricePredictionHistory.Award = 0.0m;
                        pricePredictionHistory.TotalAward = 0.0m;

                        // update user's amount
                        resolver.SysUserService.Update(pricePredictionHistory.SysUser);
                    }
                    else
                    {
                        if (pricePredictionHistory.Prediction != gameResult)
                        {
                            // update result of PrdictionHistory
                            pricePredictionHistory.Result = EnumGameResult.LOSE.ToString();
                            pricePredictionHistory.Award = 0.0m;
                            pricePredictionHistory.TotalAward = 0.0m;
                        }
                        else
                        {
                            // the amount will be awarded
                            var amountToBeAwarded = (pricePredictionHistory.Amount / totalAmountOfWinners) * totalAmountToBeAwarded; // The prize money is distributed at an equal rate according to the amount of bet.​
                            pricePredictionHistory.Result = EnumGameResult.WIN.ToString();
                            pricePredictionHistory.Award = amountToBeAwarded;
                            pricePredictionHistory.TotalAward = amountToBeAwarded + pricePredictionHistory.Amount;
                            pricePredictionHistory.SysUser.TokenAmount += amountToBeAwarded + pricePredictionHistory.Amount; // add amount award and refund amount bet

                            // update user's amount
                            resolver.SysUserService.Update(pricePredictionHistory.SysUser);
                        }
                    }

                    pricePredictionHistory.UpdatedDate = DateTime.Now;

                    // update price prediction history
                    resolver.PricePredictionHistoryService.Update(pricePredictionHistory);
                }

                // update pricePrediction
                pricePrediction.NumberOfPredictors = pricePredictionHistories.Count();
                pricePrediction.Volume = pricePredictionHistories.Sum(x => x.Amount);
                resolver.PricePredictionService.Update(pricePrediction);

                // save to DB
                resolver.UnitOfWork.SaveChanges();

                Utils.FileAppendThreadSafe(FileName, string.Format("3.1. PricePredictionId = {0} ends at: {1}{2}", pricePredictionId, DateTime.Now, Environment.NewLine));
            }
            catch (Exception ex)
            {
                if (ex.InnerException?.Message != null)
                    Utils.FileAppendThreadSafe(FileName, string.Format("  + DoUpdateWinnerPricePrediction -- Exception {0} at {1}{2}", ex.InnerException.Message, DateTime.Now, Environment.NewLine));
                else
                    Utils.FileAppendThreadSafe(FileName, string.Format("  + DoUpdateWinnerPricePrediction -- Exception {0} at {1}{2}", ex.Message, DateTime.Now, Environment.NewLine));
            }
        }
    }

    public static class EnumBooleanExtension
    {
        public static bool ToBoolean(this EnumPricePredictionStatus value)
        {
            return value == EnumPricePredictionStatus.UP;
        }
    }

}
