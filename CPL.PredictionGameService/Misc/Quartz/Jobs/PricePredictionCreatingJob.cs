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
        private static int NumberOfDailyPricePrediction;
        private static int PricePredictionBettingIntervalInHour;
        private static int HoldingIntervalInHour;
        private static int CompareIntervalInMinutes;

        public Task Execute(IJobExecutionContext context)
        {
            var resolver = new Resolver();

            NumberOfDailyPricePrediction = int.Parse(resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.NumberOfDailyPricePrediction).Value);
            PricePredictionBettingIntervalInHour = int.Parse(resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.PricePredictionBettingIntervalInHour).Value);
            HoldingIntervalInHour = int.Parse(resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.HoldingIntervalInHour).Value);
            CompareIntervalInMinutes = int.Parse(resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.CompareIntervalInMinutes).Value);

            DoCreateNewPricePrediction(resolver);
            return Task.FromResult(0);
        }

        public void DoCreateNewPricePrediction(Resolver resolver)
        {
            var startTime = DateTime.Now;

            for (int i = 0; i < NumberOfDailyPricePrediction; i++)
            {
                var newPricePredictionRecord = new PricePrediction
                {
                    Name = String.Format("Price Prediction #{0} {1}", (i + 1), startTime.ToString()),
                    Coinbase = EnumCurrenciesPair.BTCUSDT.ToString(),
                    OpenBettingTime = startTime,
                    CloseBettingTime = startTime.AddHours((i + 1) * PricePredictionBettingIntervalInHour),
                    ToBeComparedTime = startTime.AddHours((i + 1) * PricePredictionBettingIntervalInHour).AddHours(HoldingIntervalInHour),
                    ResultTime = startTime.AddHours((i + 1) * PricePredictionBettingIntervalInHour).AddHours(HoldingIntervalInHour).AddMinutes(CompareIntervalInMinutes)
                };

                resolver.PricePredictionService.Insert(newPricePredictionRecord);
            }

            resolver.UnitOfWork.SaveChanges();
        }

    }
}
