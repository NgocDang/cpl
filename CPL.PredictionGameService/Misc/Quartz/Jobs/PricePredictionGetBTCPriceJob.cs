using CPL.Common.Enums;
using CPL.Common.Misc;
using Microsoft.Extensions.PlatformAbstractions;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPL.PredictionGameService.Misc.Quartz.Jobs
{
    class PricePredictionGetBTCPriceJob : IJob
    {
        private static int CompareIntervalInMinutes;
        public string FileName { get; set; }

        public PricePredictionGetBTCPriceJob()
        {
            FileName = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "log.txt");
        }

        public Task Execute(IJobExecutionContext context)
        {
            int pricePredictionId = DoGetBTCPrizePricePrediction();
            DoUpdateWinner(pricePredictionId);
            return Task.FromResult(0);
        }

        private int DoGetBTCPrizePricePrediction()
        {
            try
            {
                var resolver = new Resolver();

                // interval time
                CompareIntervalInMinutes = int.Parse(resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.CompareIntervalInMinutes).Value);
                // currentTime
                var resultTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
                // the time to be compared
                var toBeComparedTime = ((DateTimeOffset)DateTime.UtcNow.AddMinutes(-/*CompareIntervalInMinutes*/1)).ToUnixTimeSeconds(); // TO DO
                // get price at current time from BTCPrice table
                var resultPrize = resolver.BTCPriceService.Queryable().OrderByDescending(x => x.Time).FirstOrDefault(x => resultTime >= x.Time).Price;
                // get price at time to be compared from BTCPrice table
                var toBeComparedPrize = resolver.BTCPriceService.Queryable().OrderByDescending(x => x.Time).FirstOrDefault(x => toBeComparedTime >= x.Time).Price;

                //var resultDBTime = resolver.BTCPriceService.Queryable().OrderByDescending(x => x.Time).FirstOrDefault(x => resultTime >= x.Time).Time;
                // get price at time to be compared from BTCPrice table
                //var toBeComparedDBTime = resolver.BTCPriceService.Queryable().OrderByDescending(x => x.Time).FirstOrDefault(x => toBeComparedTime >= x.Time).Time;

                // update price prediction
                var pricePrediction = resolver.PricePredictionService.Queryable().OrderBy(x => x.ResultTime).FirstOrDefault(x => !x.ResultPrice.HasValue && !x.ToBeComparedPrice.HasValue && DateTime.Now >= x.ResultTime); // TEST
                pricePrediction.ResultPrice = resultPrize;
                pricePrediction.ToBeComparedPrice = toBeComparedPrize;
                pricePrediction.UpdatedDate = DateTime.Now;

                resolver.PricePredictionService.Update(pricePrediction);

                // save to DB
                resolver.UnitOfWork.SaveChanges();

                return pricePrediction.Id;
            }
            catch (Exception ex)
            {
                if (ex.InnerException?.Message != null)
                    Utils.FileAppendThreadSafe(FileName, string.Format("DoGetBTCPrizePricePrediction -- Exception {0} at {1}{2}", ex.InnerException.Message, DateTime.Now, Environment.NewLine));
                else
                    Utils.FileAppendThreadSafe(FileName, string.Format("DoGetBTCPrizePricePrediction -- Exception {0} at {1}{2}", ex.Message, DateTime.Now, Environment.NewLine));

                return 0 ;
            }
        }

        private void DoUpdateWinner(int pricePredictionId)
        {
            try
            {
                var resolver = new Resolver();

                var pricePrediction = resolver.PricePredictionService
                    .Queryable()
                    .FirstOrDefault(x => x.Id == pricePredictionId);

                var pricePredictionHistories = resolver.PricePredictionHistoryService
                    .Query()
                    .Include(x => x.SysUser)
                    .Select()
                    .Where(x => x.Id == pricePredictionId);

                // result of game
                var gameResult = (pricePrediction.ResultPrice > pricePrediction.ToBeComparedPrice) ? EnumPricePredictionStatus.UP.ToBoolean() : EnumPricePredictionStatus.DOWN.ToBoolean(); // equals?

                // calculate the prize
                var totalAmountOfLoseUsers = pricePredictionHistories.Where(x => x.Prediction != gameResult).Sum(x => x.Amount);
                var totalAmountToBeAwarded = totalAmountOfLoseUsers * 80 / 100; // Distribute 80% of the loser's BET quantity to the winners.
                var totalAmountOfWinUsers = pricePredictionHistories.Where(x => x.Prediction == gameResult).Sum(x => x.Amount);
                
                // calculate amount of each win users and update status.
                foreach (var pricePredictionHistory in pricePredictionHistories)
                {
                    if (pricePredictionHistory.Prediction != gameResult)
                    {
                        // check minus money
                        pricePredictionHistory.Result = EnumGameResult.LOSE.ToString();
                    }
                    else
                    {
                        // the amount will be awarded
                        var amountToBeAwarded = (pricePredictionHistory.Amount / totalAmountOfWinUsers) * totalAmountToBeAwarded; // The prize money is distributed at an equal rate according to the amount of bet.​
                        pricePredictionHistory.Result = EnumGameResult.WIN.ToString();
                        pricePredictionHistory.SysUser.TokenAmount += amountToBeAwarded;
                    }
                    pricePredictionHistory.UpdatedDate = DateTime.Now;
                    
                    resolver.PricePredictionHistoryService.Update(pricePredictionHistory);
                }

                // update pricePrediction
                pricePrediction.NumberOfPredictors = pricePredictionHistories.Count();
                pricePrediction.Volume = pricePredictionHistories.Sum(x => x.Amount);
                pricePrediction.Description += $" Game end at {DateTime.Now}";

                resolver.PricePredictionService.Update(pricePrediction);

                // save to DB
                resolver.UnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException?.Message != null)
                    Utils.FileAppendThreadSafe(FileName, string.Format("DoUpdateWinnerPricePrediction -- Exception {0} at {1}{2}", ex.InnerException.Message, DateTime.Now, Environment.NewLine));
                else
                    Utils.FileAppendThreadSafe(FileName, string.Format("DoUpdateWinnerPricePrediction -- Exception {0} at {1}{2}", ex.Message, DateTime.Now, Environment.NewLine));
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
