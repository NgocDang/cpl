﻿using Autofac;
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

        public string FileName { get; set; }
        public int RunningIntervalInMilliseconds { get; set; }

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

                IJobDetail job = JobBuilder.Create<PricePredictionCreatingJob>()
                    .WithIdentity("PricePredictionCreatingJob", "QuartzGroup")
                    .WithDescription("Job to create new PricePredictions daily automatically")
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("PricePredictionCreatingJob", "QuartzGroup")
                    .WithDescription("Job to create new PricePredictions daily automatically")
                    .WithSchedule(PredictionGameServiceConstant.DailyStartTime)
                    .Build();

                await scheduler.ScheduleJob(job, trigger);
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
                    if (ex.InnerException.Message != null)
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
            FileName = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "log.txt");
            RunningIntervalInMilliseconds = int.Parse(Configuration["RunningIntervalInMilliseconds"]);
            var resolver = new Resolver();
            var cplServiceEndpoint = resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.CPLServiceEndpoint).Value;
            BTCCurrentPriceClient.Endpoint.Address = new EndpointAddress(new Uri(cplServiceEndpoint + CPLConstant.BTCCurrentPriceServiceEndpoint));
        }
    }
}
