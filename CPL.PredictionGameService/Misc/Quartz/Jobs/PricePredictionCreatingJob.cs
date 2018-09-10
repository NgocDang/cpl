using CPL.Common.Enums;
using CPL.Core.Services;
using CPL.Domain;
using CPL.Infrastructure.Repositories;
using Quartz;
using Quartz.Impl;
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
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            Resolver resolver = (Resolver)dataMap["resolve"];

            NumberOfDailyPricePrediction = int.Parse(resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.NumberOfDailyPricePrediction).Value);
            PricePredictionBettingIntervalInHour = int.Parse(resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.PricePredictionBettingIntervalInHour).Value);
            HoldingIntervalInHour = int.Parse(resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.HoldingIntervalInHour).Value);
            CompareIntervalInMinutes = int.Parse(resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.CompareIntervalInMinutes).Value);

            DoCreateNewPricePrediction(ref resolver);
            return Task.FromResult(0);
        }

        public void DoCreateNewPricePrediction(ref Resolver resolver)
        {
            var startTime = DateTime.Now;

            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            scheduler.Start();

            for (int i = 0; i < 10; i++)
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

                DateTimeOffset timeOffset = DateBuilder.DateOf(
                    newPricePredictionRecord.CloseBettingTime.Hour,
                    newPricePredictionRecord.CloseBettingTime.Minute,
                    newPricePredictionRecord.CloseBettingTime.Second,
                    newPricePredictionRecord.CloseBettingTime.Day,
                    newPricePredictionRecord.CloseBettingTime.Month,
                    newPricePredictionRecord.CloseBettingTime.Year);

                var jobData = new JobDataMap
                {
                    ["resolve"] = resolver
                };
                IJobDetail job = JobBuilder.Create<PricePredictionGetBTCPriceJob>()
                     .UsingJobData(jobData)
                    .WithIdentity($"PricePredictionUpdateBTCPrice{i}", "QuartzGroup")
                    .WithDescription("Job to update BTC price each interval hours automatically")
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity($"PricePredictionUpdateBTCPrice{i}", "QuartzGroup")
                    .WithDescription("Job to update BTC price each interval hours automatically")
                    .StartAt(timeOffset)
                    .Build();

                scheduler.ScheduleJob(job, trigger);
            }

            resolver.UnitOfWork.SaveChanges();
        }

    }
}
