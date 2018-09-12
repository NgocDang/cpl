using CPL.Common.Enums;
using CPL.Common.Misc;
using CPL.PaymentService.Misc;
using CPL.PaymentService.Misc.Quartz.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using PeterKottas.DotNetCore.WindowsService.Base;
using PeterKottas.DotNetCore.WindowsService.Interfaces;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPL.PaymentService
{
    public class CPLPaymentService : MicroService, IMicroService
    {
        public static IConfiguration Configuration { get; set; }
        public static bool IsCPLPaymentServiceRunning = false;
        public static List<Task> Tasks = new List<Task>();
        public string PaymentFileName { get; set; }
        public string PaymentProcessFileName { get; set; }
        public Resolver Resolver { get; set; }

        private static int PaymentMonthlyStartTimeInDay;
        private static int PaymentMonthlyStartTimeInHour;
        private static int PaymentMonthlyStartTimeInMinute;

        public void Start()
        {
            // ConfigurationBuilder
            ConfigurationBuilder();

            // Initialize
            Initialize();

            //Init setting
            IsCPLPaymentServiceRunning = true;
            Tasks.Clear();

            Tasks.Add(Task.Run(async () =>
            {
                IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
                await scheduler.Start();

                // Start the job as usual
                var jobData = new JobDataMap
                {
                    ["Resolver"] = Resolver
                };

                //Payment creating job
                IJobDetail creatingJob = JobBuilder.Create<PaymentCreateJob>()
                    .UsingJobData(jobData)
                    .WithIdentity("PaymentCreateJob", "QuartzGroup")
                    .WithDescription("Job to automatically create payment at the beginning of every month")
                    .Build();

                ITrigger creatingTrigger = TriggerBuilder.Create()
                    .WithIdentity("PaymentCreateJob", "QuartzGroup")
                    .WithDescription("Job to automatically create payment at the beginning of every month")
                    .WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(PaymentMonthlyStartTimeInDay, PaymentMonthlyStartTimeInHour, PaymentMonthlyStartTimeInMinute))
                    .Build();

                await scheduler.ScheduleJob(creatingJob, creatingTrigger);

                //Payment processing job
                IJobDetail processingJob = JobBuilder.Create<PaymentProcessJob>()
                    .UsingJobData(jobData)
                    .WithIdentity("PaymentProcessJob", "QuartzGroup")
                    .WithDescription("Job to automatically process payment at 10th monthly")
                    .Build();

                ITrigger processingTrigger = TriggerBuilder.Create()
                    .WithIdentity("PaymentProcessJob", "QuartzGroup")
                    .WithDescription("Job to automatically process payment at 10th monthly")
                    .WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(PaymentMonthlyStartTimeInDay, PaymentMonthlyStartTimeInHour, PaymentMonthlyStartTimeInMinute))
                    .Build();

                await scheduler.ScheduleJob(processingJob, processingTrigger);
            }));
        }

        public async void Stop()
        {
            IsCPLPaymentServiceRunning = false;
            Utils.FileAppendThreadSafe(PaymentFileName, string.Format("Stop main thread at : {0}{1}{2}", DateTime.Now, Environment.NewLine, Environment.NewLine));

            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Shutdown();
            Utils.FileAppendThreadSafe(PaymentFileName, string.Format("Scheduler shutdown ({0}) at : {1}{2}{3}", scheduler.IsShutdown, DateTime.Now, Environment.NewLine, Environment.NewLine));

            Task.WaitAll(Tasks.ToArray());
        }

        // ConfigurationBuilder
        private void ConfigurationBuilder()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = configBuilder.Build();
        }

        // Initialize
        private void Initialize()
        {
            Resolver = new Resolver();
            PaymentFileName = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "log.txt");
        }
    }
}
