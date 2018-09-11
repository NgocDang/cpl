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
        private static int PricePredictionBettingIntervalInHour;
        private static int HoldingIntervalInHour;
        private static int CompareIntervalInMinute;

        public Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            Resolver resolver = (Resolver)dataMap["Resolver"];

            PricePredictionBettingIntervalInHour = int.Parse(resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.PricePredictionBettingIntervalInHour).Value);
            HoldingIntervalInHour = int.Parse(resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.HoldingIntervalInHour).Value);
            CompareIntervalInMinute = int.Parse(resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.CompareIntervalInMinute).Value);

            DoCreatePricePrediction(ref resolver);
            return Task.FromResult(0);
        }

        public void DoCreatePricePrediction(ref Resolver resolver)
        {
            var startTime = DateTime.Now.Date;
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            scheduler.Start();
            var lstResultTime = new List<DateTime>();
            var lstToBeComparedTime = new List<DateTime>();
            var lstId = new List<int> ();
            for (int i = 0; i < 24/ PricePredictionBettingIntervalInHour; i++)
            {
                var newPricePredictionRecord = new PricePrediction
                {
                    Name = String.Format("Price Prediction #{0} {1}", (i + 1), startTime.ToString()),
                    Coinbase = EnumCurrencyPair.BTCUSDT.ToString(),
                    OpenBettingTime = startTime,
                    CloseBettingTime = startTime.AddHours((i + 1) * PricePredictionBettingIntervalInHour),
                    ToBeComparedTime = startTime.AddHours((i + 1) * PricePredictionBettingIntervalInHour).AddHours(HoldingIntervalInHour),
                    ResultTime = startTime.AddHours((i + 1) * PricePredictionBettingIntervalInHour).AddHours(HoldingIntervalInHour).AddMinutes(CompareIntervalInMinute)
                };

                resolver.PricePredictionService.Insert(newPricePredictionRecord);

                // udpate DB
                resolver.UnitOfWork.SaveChanges();

                // add time to start the job
                lstResultTime.Add(newPricePredictionRecord.ResultTime);
                lstToBeComparedTime.Add(newPricePredictionRecord.ToBeComparedTime);
                lstId.Add(newPricePredictionRecord.Id);
            }

            for (int i = 0; i < lstId.Count; i++)
            {
                DateTimeOffset timeOffset = DateBuilder.DateOf(
                                        lstResultTime[i].Hour,
                                        lstResultTime[i].Minute,
                                        lstResultTime[i].Second,
                                        lstResultTime[i].Day,
                                        lstResultTime[i].Month,
                                        lstResultTime[i].Year);

                var jobData = new JobDataMap
                {
                    ["Resolver"] = resolver,
                    ["ResultTime"] = lstResultTime[i],
                    ["ToBeComparedTime"] = lstToBeComparedTime[i],
                };
                IJobDetail job = JobBuilder.Create<PricePredictionGetBTCPriceJob>()
                     .UsingJobData(jobData)
                    .WithIdentity($"PricePredictionUpdateBTCPrice{lstId[i]}", "QuartzGroup")
                    .WithDescription("Job to update BTC price each interval hours automatically")
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity($"PricePredictionUpdateBTCPrice{lstId[i]}", "QuartzGroup")
                    .WithDescription("Job to update BTC price each interval hours automatically")
                    .StartAt(timeOffset)
                    .Build();

                scheduler.ScheduleJob(job, trigger);
            }
        }

    }
}
