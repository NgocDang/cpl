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
        }

        public static void AddJob<TJob>(IScheduler scheduler, DateTime dateTime)
            where TJob : IJob
        {
            var jobName = $"{dateTime.ToString("yyyyMMddHHmmss")}.job";
            var jobGroupName = $"{typeof(TJob).FullName}.jobGroup";

            var triggerName = $"{dateTime.ToString("yyyyMMddHHmmss")}.trigger";

            var job = JobBuilder.Create<TJob>()
                .WithIdentity(jobName, jobGroupName)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity(triggerName)
                .StartAt(dateTime)
                .Build();

            scheduler.ScheduleJob(job, trigger);

            var jobKey = new JobKey(jobName);

            var jobDetailResult = scheduler.GetJobDetail(jobKey);
            jobDetailResult.Wait();
            var jobDetail = jobDetailResult.Result;
        }

        private static CronScheduleBuilder BuildCronSchedule(DateTime dateTime)
        {
            return CronScheduleBuilder.DailyAtHourAndMinute(dateTime.Hour, dateTime.Minute);
        }
    }
}
