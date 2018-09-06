using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPL.PredictionGameService.Misc.Quartz.Jobs
{
    class PricePredictionGetBTCPriceJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            DoPricePredictionGetBTCPrize();
            return Task.FromResult(0);
        }

        private void DoPricePredictionGetBTCPrize()
        {
            var resolver = new Resolver();

            var pricePrediction = resolver.PricePredictionService.Queryable().FirstOrDefault(x => !x.ResultPrice.HasValue && DateTime.Now >= x.ResultTime); // TEST

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
