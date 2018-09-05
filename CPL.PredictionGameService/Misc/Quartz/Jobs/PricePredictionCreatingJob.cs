using CPL.Common.Enums;
using CPL.Core.Services;
using CPL.Domain;
using CPL.Infrastructure.Repositories;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPL.PredictionGameService.Misc.Quartz.Jobs
{
    internal class PricePredictionCreatingJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var resolver = new Resolver();
            DoCreateNewPricePrediction(resolver);
            return Task.FromResult(0);
        }

        public void DoCreateNewPricePrediction(Resolver resolver)
        {
            var startTime = DateTime.Now;

            for (int i = 0; i < PredictionGameServiceConstant.NumberOfDailyPricePrediction; i++)
            {
                var newPricePredictionRecord = new PricePrediction
                {
                    Name = String.Format("Price Prediction #{0} {1}", (i + 1), startTime.ToString()),
                    Coinbase = EnumCurrenciesPair.BTCUSDT.ToString(),
                    OpenBettingTime = startTime,
                    CloseBettingTime = startTime.AddHours((i + 1) * PredictionGameServiceConstant.PricePredictionBettingIntervalInHour),
                    ToBeComparedTime = startTime.AddHours((i + 1) * PredictionGameServiceConstant.PricePredictionBettingIntervalInHour).AddHours(PredictionGameServiceConstant.HoldingIntervalInHour),
                    ResultTime = startTime.AddHours((i + 1) * PredictionGameServiceConstant.PricePredictionBettingIntervalInHour).AddHours(PredictionGameServiceConstant.HoldingIntervalInHour).AddMinutes(PredictionGameServiceConstant.CompareIntervalInMinutes)
                };

                resolver.PricePredictionService.Insert(newPricePredictionRecord);
            }

            resolver.UnitOfWork.SaveChanges();
        }

    }
}
