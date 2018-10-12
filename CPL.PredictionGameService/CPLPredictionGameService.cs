using BTCCurrentPriceService;
using CPL.Common.Enums;
using CPL.Common.Misc;
using CPL.Domain;
using CPL.PredictionGameService.Misc;
using CPL.PredictionGameService.Misc.Quartz.Jobs;
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
using System.ServiceModel;
using System.Threading.Tasks;

namespace CPL.PredictionGameService
{
    public class CPLPredictionGameService : MicroService, IMicroService
    {
        public static IConfiguration Configuration { get; set; }
        public static BTCCurrentPriceClient BTCCurrentPriceClient = new BTCCurrentPriceClient();
        public static bool IsCPLPredictionGameServiceRunning = false;
        public static List<Task> Tasks = new List<Task>();
        private static int SystemPricePredictionBettingIntervalInHour;        // 8h
        private static int SystemPricePredictionHoldingIntervalInHour;        // 1h
        private static int SystemPricePredictionCompareIntervalInMinute;      // 15m

        public int RunningIntervalInMilliseconds { get; set; }

        public string GeneralLogFileName { get; set; }
        public string SystemLogFileName { get; set; }
        public string AdminLogFileName { get; set; }

        public Resolver SystemResolver { get; set; }
        public Resolver AdminResolver { get; set; }
        public BasePricePredictionFunctions SystemBasePricePredictionFunctions { get; set; }
        public BasePricePredictionFunctions AdminBasePricePredictionFunctions { get; set; }

        public void Start()
        {
            // ConfigurationBuilder
            ConfigurationBuilder();

            // Initialize
            Initialize();

            // write log
            Utils.FileAppendThreadSafe(GeneralLogFileName, String.Format("{0} started at {1}{2}", PredictionGameServiceConstant.ServiceName, DateTime.Now, Environment.NewLine));

            //Init setting
            IsCPLPredictionGameServiceRunning = true;
            Tasks.Clear();

            Tasks.Add(Task.Run(() => GetCurrentBTCPrice()));

            Tasks.Add(Task.Run(async () =>
            {
                // SYSTEM PRICE PREDICTION CREATING JOB
                var systemStartHour = SystemPricePredictionHoldingIntervalInHour;       // 1h
                var systemStartMinute = SystemPricePredictionCompareIntervalInMinute;   // 15m

                IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
                await scheduler.Start();

                var systemJobData = new JobDataMap
                {
                    ["Resolver"] = SystemResolver,
                    ["SystemBasePricePredictionFunctions"] = SystemBasePricePredictionFunctions,
                    ["SystemLogFileName"] = SystemLogFileName
                };

                IJobDetail systemPricePredictionCreatingJob = JobBuilder.Create<SystemPricePredictionJob>()
                    .UsingJobData(systemJobData)
                    .WithIdentity(new JobKey("SystemPricePredictionCreatingJob", "QuartzGroup"))
                    .WithDescription("Job to create new System PricePredictions daily automatically")
                    .Build();

                ITrigger systemPricePredictionCreatingTrigger = TriggerBuilder.Create()
                    .WithIdentity(new TriggerKey("SystemPricePredictionCreatingJob", "QuartzGroup"))
                    .WithDescription("Job to create new System PricePredictions daily automatically")
                    .WithDailyTimeIntervalSchedule(x => x.WithIntervalInHours(SystemPricePredictionBettingIntervalInHour)
                        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(systemStartHour, systemStartMinute)))
                    .Build();
                await scheduler.ScheduleJob(systemPricePredictionCreatingJob, systemPricePredictionCreatingTrigger);


                // ADMIN PRICE PREDICTION CREATING JOB
                var adminStartHour = int.Parse(SystemResolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.AdminPricePredictionDailyJobStartHour).Value);   // 0h
                var adminStartMinute = int.Parse(SystemResolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.AdminPricePredictionDailyJobStartMinute).Value);  // 0m

                var adminJobData = new JobDataMap
                {
                    ["Resolver"] = AdminResolver,
                    ["AdminBasePricePredictionFunctions"] = AdminBasePricePredictionFunctions,
                    ["AdminLogFileName"] = AdminLogFileName
                };

                IJobDetail adminPricePredictionCreatingJob = JobBuilder.Create<AdminPricePredictionJob>()
                    .UsingJobData(adminJobData)
                    .WithIdentity(new JobKey("AdminPricePredictionCreatingJob", "QuartzGroup"))
                    .WithDescription("Job to create new admin's PricePredictions daily automatically")
                    .Build();

                ITrigger adminPricePredictionCreatingTrigger = TriggerBuilder.Create()
                    .WithIdentity(new TriggerKey("AdminPricePredictionCreatingJob", "QuartzGroup"))
                    .WithDescription("Job to create new admin's PricePredictions daily automatically")
                    .WithDailyTimeIntervalSchedule(x => x.WithIntervalInHours(PredictionGameServiceConstant.DailyIntervalInHours)
                        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(adminStartHour, adminStartMinute)))
                    .Build();

                await scheduler.ScheduleJob(adminPricePredictionCreatingJob, adminPricePredictionCreatingTrigger);

                // Admin's PricePredictions check result job
                // TODO: use Status column instead of checking ResultPrice.HasValue
                var activeAdminPricePredictions = SystemResolver.PricePredictionService.Queryable().Where(x => !x.ResultPrice.HasValue && !x.ToBeComparedPrice.HasValue && x.IsCreatedByAdmin && x.ResultTime > DateTime.Now).ToList();
                foreach (var activeAdminPricePrediction in activeAdminPricePredictions)
                {
                    adminJobData = new JobDataMap
                    {
                        ["Resolver"] = AdminResolver,
                        ["PricePredictionId"] = activeAdminPricePrediction.Id,
                        ["AdminBasePricePredictionFunctions"] = AdminBasePricePredictionFunctions,
                        ["AdminLogFileName"] = AdminLogFileName
                    };

                    IJobDetail adminPricePredictionCheckResultJob = JobBuilder.Create<AdminPricePredictionCheckResultJob>()
                        .UsingJobData(adminJobData)
                        .WithIdentity(new JobKey(string.Format("AdminPricePredictionId{0}CheckResultJob", activeAdminPricePrediction.Id), "QuartzGroup"))
                        .WithDescription("Job to check admin's PricePredictions result")
                        .Build();

                    ITrigger adminPricePredictionCheckResultTrigger = TriggerBuilder.Create()
                        .WithIdentity(new TriggerKey(string.Format("AdminPricePredictionId{0}CheckResultJob", activeAdminPricePrediction.Id), "QuartzGroup"))
                        .WithDescription("Job to check admin's PricePredictions result")
                        .WithDailyTimeIntervalSchedule(x => x.WithIntervalInHours(PredictionGameServiceConstant.DailyIntervalInHours) // If not doing this, job will be triggered with 1 min interval
                            .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(activeAdminPricePrediction.ResultTime.Hour, activeAdminPricePrediction.ResultTime.Minute))
                            .WithRepeatCount(0))
                        .Build();
                    await scheduler.ScheduleJob(adminPricePredictionCheckResultJob, adminPricePredictionCheckResultTrigger);
                }

            }));
        }

        public async void Stop()
        {
            IsCPLPredictionGameServiceRunning = false;
            Utils.FileAppendThreadSafe(GeneralLogFileName, string.Format("Stop main thread at : {0}{1}{2}", DateTime.Now, Environment.NewLine, Environment.NewLine));

            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Shutdown();
            Utils.FileAppendThreadSafe(GeneralLogFileName, string.Format("Scheduler shutdown ({0}) at : {1}{2}{3}", scheduler.IsShutdown, DateTime.Now, Environment.NewLine, Environment.NewLine));

            Task.WaitAll(Tasks.ToArray());
        }

        // Get current BTC price
        private void GetCurrentBTCPrice()
        {
            var resolver = new Resolver();

            Utils.FileAppendThreadSafe(GeneralLogFileName, string.Format("Get current BTC thread on CPL window service STARTED on {0}{1}", DateTime.Now, Environment.NewLine));
            while (IsCPLPredictionGameServiceRunning)
            {
                try
                {
                    var btcCurrentPriceResult = BTCCurrentPriceClient.SetBTCCurrentPriceAsync();
                    btcCurrentPriceResult.Wait();

                    if (btcCurrentPriceResult.Result.Status.Code != 0)
                    {
                        Utils.FileAppendThreadSafe(GeneralLogFileName, string.Format("Get current BTC failed. Reason: {0}{1}", btcCurrentPriceResult.Result.Status.Text, Environment.NewLine));
                        return;
                    }

                    var btcPrice = new BTCPrice()
                    {
                        Price = btcCurrentPriceResult.Result.Price,
                        Time = btcCurrentPriceResult.Result.DateTime
                    };

                    resolver.BTCPriceService.Insert(btcPrice);
                    resolver.UnitOfWork.SaveChanges();

                    Task.Delay(RunningIntervalInMilliseconds).Wait();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException?.Message != null)
                        Utils.FileAppendThreadSafe(GeneralLogFileName, string.Format("Exception {0} at {1}{2}", ex.InnerException.Message, DateTime.Now, Environment.NewLine));
                    else
                        Utils.FileAppendThreadSafe(GeneralLogFileName, string.Format("Exception {0} at {1}{2}", ex.Message, DateTime.Now, Environment.NewLine));
                }
            }
            Utils.FileAppendThreadSafe(GeneralLogFileName, string.Format("Get current BTC thread on CPL window service STOPPED at {0}{1}", DateTime.Now, Environment.NewLine));
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
            SystemResolver = new Resolver();
            AdminResolver = new Resolver();
            SystemBasePricePredictionFunctions = new BasePricePredictionFunctions();
            AdminBasePricePredictionFunctions = new BasePricePredictionFunctions();

            GeneralLogFileName = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "log.txt");
            SystemLogFileName = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "systemLog.txt");
            AdminLogFileName = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "adminLog.txt");

            RunningIntervalInMilliseconds = int.Parse(Configuration["RunningIntervalInMilliseconds"]);
            var cplServiceEndpoint = SystemResolver.SettingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.CPLServiceEndpoint).Value;
            SystemPricePredictionBettingIntervalInHour = int.Parse(SystemResolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.PricePredictionBettingIntervalInHour).Value);
            SystemPricePredictionHoldingIntervalInHour = int.Parse(SystemResolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.PricePredictionHoldingIntervalInHour).Value);
            SystemPricePredictionCompareIntervalInMinute = int.Parse(SystemResolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.PricePredictionCompareIntervalInMinute).Value);
            BTCCurrentPriceClient.Endpoint.Address = new EndpointAddress(new Uri(cplServiceEndpoint + CPLConstant.BTCCurrentPriceServiceEndpoint));
        }
    }
}
