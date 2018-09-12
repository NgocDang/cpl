using Autofac;
using BTCCurrentPriceService;
using CPL.Common.Enums;
using CPL.Common.Misc;
using CPL.Core.Interfaces;
using CPL.Core.Services;
using CPL.Domain;
using CPL.Infrastructure;
using CPL.Infrastructure.Interfaces;
using CPL.Infrastructure.Repositories;
using CPL.PredictionGameService.Misc;
using CPL.PredictionGameService.Misc.Quartz.Jobs;
using Microsoft.EntityFrameworkCore;
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CPL.PredictionGameService
{
    public class CPLPredictionGameService : MicroService, IMicroService
    {
        public static IConfiguration Configuration { get; set; }
        public static BTCCurrentPriceClient BTCCurrentPriceClient = new BTCCurrentPriceClient();
        public static bool IsCPLPredictionGameServiceRunning = false;
        public static List<Task> Tasks = new List<Task>();
        private static int PricePredictionDailyStartTimeInHour;
        private static int PricePredictionDailyStartTimeInMinute;

        public string FileName { get; set; }
        public int RunningIntervalInMilliseconds { get; set; }

        Resolver Resolver { get; set; }

        public void Start()
        {
            // ConfigurationBuilder
            ConfigurationBuilder();

            // Initialize
            Initialize();

            // write log
            Utils.FileAppendThreadSafe(FileName, String.Format("{0} started at {1}{2}", PredictionGameServiceConstant.ServiceName, DateTime.Now, Environment.NewLine));

            //Init setting
            IsCPLPredictionGameServiceRunning = true;
            Tasks.Clear();

            Tasks.Add(Task.Run(() => GetCurrentBTCPrice()));

            Tasks.Add(Task.Run(async () =>
            {
                IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
                await scheduler.Start();
                
                // Work on pending job since service starts
                var pricePredictions = Resolver.PricePredictionService.Queryable().OrderBy(x => x.ResultTime)
                    .Where(x => !x.ResultPrice.HasValue && !x.ToBeComparedPrice.HasValue);

                foreach (var pricePrediction in pricePredictions)
                {
                    var timeOffset = DateBuilder.DateOf(
                                            pricePrediction.ResultTime.Hour,
                                            pricePrediction.ResultTime.Minute,
                                            pricePrediction.ResultTime.Second,
                                            pricePrediction.ResultTime.Day,
                                            pricePrediction.ResultTime.Month,
                                            pricePrediction.ResultTime.Year);

                    var jobData = new JobDataMap
                    {
                        ["Resolver"] = Resolver,
                        ["ResultTime"] = pricePrediction.ResultTime,
                        ["ToBeComparedTime"] = pricePrediction.ToBeComparedTime,
                    };

                    IJobDetail job = JobBuilder.Create<PricePredictionGetBTCPriceJob>()
                         .UsingJobData(jobData)
                        .WithIdentity($"PricePredictionUpdateBTCPrice{pricePrediction.Id}", "QuartzGroup")
                        .WithDescription("Job to update BTC price each interval hours automatically")
                        .Build();

                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity($"PricePredictionUpdateBTCPrice{pricePrediction.Id}", "QuartzGroup")
                        .WithDescription("Job to update BTC price each interval hours automatically")
                        .StartAt(timeOffset)
                        .Build();

                    await scheduler.ScheduleJob(job, trigger);
                }

                // Start the job as usual
                var creatingJobData = new JobDataMap
                {
                    ["Resolver"] = Resolver
                };

                IJobDetail creatingJob = JobBuilder.Create<PricePredictionCreatingJob>()
                    .UsingJobData(creatingJobData)
                    .WithIdentity("PricePredictionCreatingJob", "QuartzGroup")
                    .WithDescription("Job to create new PricePredictions daily automatically")
                    .Build();

                ITrigger creatingTrigger = TriggerBuilder.Create()
                    .WithIdentity("PricePredictionCreatingJob", "QuartzGroup")
                    .WithDescription("Job to create new PricePredictions daily automatically")
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(PricePredictionDailyStartTimeInHour, PricePredictionDailyStartTimeInMinute))
                    .Build();

                await scheduler.ScheduleJob(creatingJob, creatingTrigger);
            }));
        }

        public async void Stop()
        {
            IsCPLPredictionGameServiceRunning = false;
            Utils.FileAppendThreadSafe(FileName, string.Format("Stop main thread at : {0}{1}{2}", DateTime.Now, Environment.NewLine, Environment.NewLine));

            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Shutdown();
            Utils.FileAppendThreadSafe(FileName, string.Format("Scheduler shutdown ({0}) at : {1}{2}{3}", scheduler.IsShutdown, DateTime.Now, Environment.NewLine, Environment.NewLine));

            Task.WaitAll(Tasks.ToArray());
        }

        // Get current BTC price
        private void GetCurrentBTCPrice()
        {
            var resolver = new Resolver();

            Utils.FileAppendThreadSafe(FileName, string.Format("Get current BTC thread on CPL window service STARTED on {0}{1}", DateTime.Now, Environment.NewLine));
            while (IsCPLPredictionGameServiceRunning)
            {
                try
                {
                    var btcCurrentPriceResult = BTCCurrentPriceClient.SetBTCCurrentPriceAsync();
                    btcCurrentPriceResult.Wait();

                    if (btcCurrentPriceResult.Result.Status.Code != 0)
                    {
                        Utils.FileAppendThreadSafe(FileName, string.Format("Get current BTC failed. Reason: {0}{1}", btcCurrentPriceResult.Result.Status.Text, Environment.NewLine));
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
                        Utils.FileAppendThreadSafe(FileName, string.Format("Exception {0} at {1}{2}", ex.InnerException.Message, DateTime.Now, Environment.NewLine));
                    else
                        Utils.FileAppendThreadSafe(FileName, string.Format("Exception {0} at {1}{2}", ex.Message, DateTime.Now, Environment.NewLine));
                }
            }
            Utils.FileAppendThreadSafe(FileName, string.Format("Get current BTC thread on CPL window service STOPPED at {0}{1}", DateTime.Now, Environment.NewLine));
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

            FileName = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "log.txt");
            RunningIntervalInMilliseconds = int.Parse(Configuration["RunningIntervalInMilliseconds"]);
            var cplServiceEndpoint = Resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.CPLServiceEndpoint).Value;
            PricePredictionDailyStartTimeInHour = int.Parse(Resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.PricePredictionDailyStartTimeInHour).Value);
            PricePredictionDailyStartTimeInMinute = int.Parse(Resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.PricePredictionDailyStartTimeInMinute).Value);
            BTCCurrentPriceClient.Endpoint.Address = new EndpointAddress(new Uri(cplServiceEndpoint + CPLConstant.BTCCurrentPriceServiceEndpoint));
        }
    }
}
