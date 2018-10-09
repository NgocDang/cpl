using CPL.Common.Enums;
using CPL.Common.Misc;
using CPL.Domain;
using Quartz;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CPL.Misc.Enums;
using Quartz.Impl;

namespace CPL.PredictionGameService.Misc.Quartz.Jobs
{
    internal class AdminPricePredictionJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Utils.FileAppendThreadSafe(CPLPredictionGameService.basePricePredictionFunctions.FileName, string.Format("{0}Execute AdminPricePredictionCreatingJob {1}: {2}", Environment.NewLine, DateTime.Now, Environment.NewLine));

            JobDataMap dataMap = context.JobDetail.JobDataMap;
            Resolver resolver = (Resolver)dataMap["Resolver"];

            DateTime localDateTime = context.FireTimeUtc.LocalDateTime;

            // Create new price prediction game
            DoCreatePricePrediction(ref resolver, localDateTime);

            return Task.FromResult(0);
        }

        public void DoCreatePricePrediction(ref Resolver resolver, DateTime localDateTime)
        {
            Utils.FileAppendThreadSafe(CPLPredictionGameService.basePricePredictionFunctions.FileName, string.Format("1. DoCreateAdminPricePrediction: {0}{1}", DateTime.Now, Environment.NewLine));

            var activePricePredictionSettings = resolver.PricePredictionSettingService.Queryable().Where(x => x.Status == (int)EnumPricePredictionSettingStatus.ACTIVE).ToList();

            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;

            foreach (var activePricePredictionSetting in activePricePredictionSettings)
            {
                var openBettingTime = new DateTime(localDateTime.Year, localDateTime.Month, localDateTime.Day) + activePricePredictionSetting.OpenBettingTime;
                var closeBettingTime = new DateTime(localDateTime.Year, localDateTime.Month, localDateTime.Day) + activePricePredictionSetting.CloseBettingTime;
                var toBeComparedTime = closeBettingTime.AddHours((double)activePricePredictionSetting.HoldingTimeInterval);
                var resultTime = toBeComparedTime.AddMinutes((double)activePricePredictionSetting.ResultTimeInterval);

                var newPricePredictionRecord = new PricePrediction
                {
                    Coinbase = EnumCurrencyPair.BTCUSDT.ToString(),
                    OpenBettingTime = openBettingTime,
                    CloseBettingTime = closeBettingTime,
                    ToBeComparedTime = toBeComparedTime,
                    ResultTime = resultTime,
                    IsCreatedByAdmin = true,
                    PricePredictionCategoryId = 2 // default standard priceprediction category
                };

                resolver.PricePredictionService.Insert(newPricePredictionRecord);
                resolver.UnitOfWork.SaveChanges();

                var LangIds = resolver.LangService.Queryable().Select(x => x.Id).ToList();
                //var title = newPricePredictionRecord.CloseBettingTime.ToString("HH:mm") == "00:00" ? "24:00" : newPricePredictionRecord.CloseBettingTime.ToString("HH:mm");
                var title = resolver.PricePredictionSettingDetailService.Queryable().FirstOrDefault(x => x.PricePredictionSettingId == activePricePredictionSetting.Id).Title;
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

                var jobData = new JobDataMap
                {
                    ["Resolver"] = resolver,
                    ["PricePredictionId"] = newPricePredictionRecord.Id
                };

                IJobDetail job = JobBuilder.Create<AdminPricePredictionCheckResultJob>()
                    .UsingJobData(jobData)
                    .WithIdentity(new JobKey (string.Format("AdminPricePrediction{0}CheckResultJob", newPricePredictionRecord.Id.ToString()),"QuartzGroup"))
                    .WithDescription("Job to check admin's PricePredictions result")
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity(new TriggerKey(string.Format("AdminPricePrediction{0}CheckResultJob", newPricePredictionRecord.Id.ToString()), "QuartzGroup"))
                    .WithDescription("Job to check admin's PricePredictions result")
                    .WithDailyTimeIntervalSchedule(x => x.WithIntervalInHours(PredictionGameServiceConstant.DailyIntervalInHours)
                                                        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(resultTime.Hour, resultTime.Minute))
                                                            .WithRepeatCount(0))
                    .Build();

                scheduler.ScheduleJob(job, trigger);
            }
        }
    }
}
