using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc.Quartz
{
    public class QuartzHelper
    {
        public static void StartJob<TJob>(IScheduler scheduler, DateTime dateTime)
            where TJob : IJob
        {
            var jobName = typeof(TJob).FullName;

            var job = JobBuilder.Create<TJob>()
                .WithIdentity(jobName)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"{jobName}.trigger", $"{jobName}.triggerGroup")
                .StartNow()
                .WithSchedule(BuildCronSchedule(dateTime))
                .Build();

            scheduler.ScheduleJob(job, trigger);
            var jobKey = new JobKey(jobName);

            var jobDetailResult = scheduler.GetJobDetail(jobKey);
            jobDetailResult.Wait();
            var jobDetail = jobDetailResult.Result;
        }

        public static void AddJob<TJob>(IScheduler scheduler)
            where TJob : IJob
        {
            var jobName = typeof(TJob).FullName;

            var job = JobBuilder.Create<TJob>()
                .WithIdentity(jobName)
                .Build();

            scheduler.AddJob(job, true);

            var jobKey = new JobKey(jobName);

            var jobDetailResult = scheduler.GetJobDetail(jobKey);
            jobDetailResult.Wait();
            var jobDetail = jobDetailResult.Result;

        }

        public static void TriggerForJobAtTime<TJob>(IScheduler scheduler, DateTime dateTime)
            where TJob : IJob
        {
            var jobName = typeof(TJob).FullName;
            var jobKey = new JobKey(jobName);

            var jobDetailResult = scheduler.GetJobDetail(jobKey);
            jobDetailResult.Wait();
            var jobDetail = jobDetailResult.Result;

            var triggerName = $"{dateTime.ToString("yyyyMMddHHmmss")}.trigger";
            var triggerGroupName = $"{jobName}.triggerGroup";

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"{triggerName}.trigger", triggerGroupName)
                .StartAt(dateTime)
                .ForJob(jobDetail)
                .Build();

            var triggerExistedResult = scheduler.GetTriggersOfJob(jobKey);
            triggerExistedResult.Wait();

            if (!triggerExistedResult.Result.Any(x => x.Key == trigger.Key))
            {
                scheduler.ScheduleJob(trigger);
            }
        }

        private static CronScheduleBuilder BuildCronSchedule(DateTime dateTime)
        {
            return CronScheduleBuilder.DailyAtHourAndMinute(dateTime.Hour, dateTime.Minute);
        }
    }
}
