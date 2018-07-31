using AutoMapper;
using CPL.Common.Enums;
using CPL.Core.Services;
using CPL.Domain;
using CPL.Infrastructure.Repositories;
using CPL.Misc.Quartz.Interfaces;
using CPL.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc.Quartz.Jobs
{
    public class PricePredictionUpdateResultJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            BTCPricePredictionUpdateResult(context);
            return Task.FromResult(0);
        }

        public void BTCPricePredictionUpdateResult(IJobExecutionContext context)
        {
            var btcPriceService = ((BTCPriceService)context.JobDetail.JobDataMap["BTCPriceService"]);
            var sysUserService = ((SysUserService)context.JobDetail.JobDataMap["SysUserService"]);
            var pricePredictionService = ((PricePredictionService)context.JobDetail.JobDataMap["PricePredictionService"]);
            var pricePredictionHistoryService = ((PricePredictionHistoryService)context.JobDetail.JobDataMap["PricePredictionHistoryService"]);
            var unitOfWork = ((UnitOfWork)context.JobDetail.JobDataMap["UnitOfWork"]);

            var listOfPricePrediction = pricePredictionService.Query()
                                    .Include(x => x.PricePredictionHistories)
                                    .Select()
                                    .Where(x => x.UpdatedDate != null)
                                    .Select(x => Mapper.Map<PricePredictionViewModel>(x));

            var listOfSysUser = sysUserService.Queryable()
                                .Where(x => x.KYCVerified.HasValue);

            foreach (var pricePrediction in listOfPricePrediction)
            {
                var listUserId = pricePrediction.PricePredictionHistories.Select(x => x.SysUserId).Distinct();
                var lstSysUser = listOfSysUser.Where(x => listUserId.Contains(x.Id)).ToList();

                var resultPrice = btcPriceService.Queryable()
                    .Where(x => x.Time == ((DateTimeOffset)pricePrediction.PredictionResultTime).ToUnixTimeSeconds())
                    .FirstOrDefault().Price;

                var updatedPricePrediction = UpdateResult(pricePrediction, lstSysUser, resultPrice);

                var history = updatedPricePrediction.PricePredictionHistories.OrderBy(x => x.Id).ToList();

                var dataHistory = pricePredictionHistoryService.Queryable()
                                    .Where(x => x.PricePredictionId == pricePrediction.Id)
                                    .OrderBy(x => x.Id)
                                    .ToList();

                // history
                for (var i = 0; i < history.Count(); i++)
                {
                    dataHistory[i].Result = history[i].Result;
                    dataHistory[i].Award = history[i].Award;
                    dataHistory[i].UpdatedDate = DateTime.Now;
                    pricePredictionHistoryService.Update(dataHistory[i]);
                }

                // price prediction
                var pricePredictionStatisticed = GameStatistic(updatedPricePrediction, resultPrice);

                var dataGame = pricePredictionService.Queryable()
                                .Where(x => x.Id == pricePrediction.Id)
                                .FirstOrDefault();

                dataGame.ResultPrice = pricePredictionStatisticed.ResultPrice;
                dataGame.NumberOfPredictors = pricePredictionStatisticed.NumberOfPredictors;
                dataGame.Volume = pricePredictionStatisticed.NumberOfPredictors;
                dataGame.UpdatedDate = DateTime.Now;

                pricePredictionService.Update(dataGame);
            }

            unitOfWork.SaveChanges();
        }

        private PricePredictionViewModel UpdateResult(PricePredictionViewModel pricePrediction, List<SysUser> listOfSysUser, decimal resultPrice)
        {
            foreach (var history in pricePrediction.PricePredictionHistories)
            {
                var isWinner = IsWinner(pricePrediction.PredictionPrice, resultPrice, history.Prediction);
                if (isWinner)
                {
                    if (IsKYCVerified(listOfSysUser, history.SysUserId))
                        history.Result = EnumGameResult.WIN.ToString();
                    else
                        history.Result = EnumGameResult.KYC_PENDING.ToString();
                }
                else
                {
                    history.Result = EnumGameResult.LOSE.ToString();
                    history.Award = 0.0m;
                }
            }

            var numberOfLoser = pricePrediction.PricePredictionHistories.Count(x => x.Result.Equals(EnumGameResult.LOSE.ToString()));

            return pricePrediction;
        }

        private PricePredictionViewModel GameStatistic(PricePredictionViewModel pricePrediction, decimal resultPrice)
        {
            pricePrediction.ResultPrice = resultPrice;
            pricePrediction.NumberOfPredictors = pricePrediction.PricePredictionHistories.Count;
            pricePrediction.Volume = pricePrediction.PricePredictionHistories.Sum(x => x.Amount);

            return pricePrediction;
        }

        private bool IsWinner(decimal predictionPrice, decimal resultPrice, bool prediction)
        {
            if (resultPrice >= predictionPrice && prediction)
                return true;
            else if (resultPrice >= predictionPrice && !prediction)
                return false;
            else if (resultPrice < predictionPrice && prediction)
                return false;
            else
                return true;
        }

        private bool IsKYCVerified(List<SysUser> listOfSysUser, int sysUserId)
        {
            return listOfSysUser.Any(x => x.Id == sysUserId);
        }
    }
}
