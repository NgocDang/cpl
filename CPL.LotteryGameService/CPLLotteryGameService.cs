using Autofac;
using AutoMapper;
using CPL.Common.Enums;
using CPL.Core.Interfaces;
using CPL.Core.Services;
using CPL.Domain;
using CPL.Infrastructure;
using CPL.Infrastructure.Interfaces;
using CPL.Infrastructure.Repositories;
using CPL.LotteryGameService.Misc;
using CPL.LotteryGameService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using PeterKottas.DotNetCore.WindowsService.Base;
using PeterKottas.DotNetCore.WindowsService.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CPL.LotteryGameService
{
    public class CPLLotteryGameService : MicroService, IMicroService
    {
        public static IConfiguration Configuration { get; set; }
        public static bool IsCPLLotteryGameServiceRunning = false;
        public static List<Task> Tasks = new List<Task>();

        public string FileName { get; set; }
        public string ConnectionString { get; set; }
        public int RunningIntervalInMilliseconds { get; set; }

        public void Start()
        {
            // ConfigurationBuilder
            ConfigurationBuilder();

            // Initialize
            Initialize();

            // write log
            Utils.FileAppendThreadSafe(FileName, String.Format("{0} started at {1}{2}", WSConstant.ServiceName, DateTime.Now, Environment.NewLine));

            //Init dependency transaction & dbcontext
            Repository();

            // AutoMapper Initialize
            AutoMapperInitialize();

            //Init setting
            IsCPLLotteryGameServiceRunning = true;
            Tasks.Clear();
            ABC();
            //Tasks.Add(Task.Run(() => ABC()));
        }

        public void Stop()
        {
            IsCPLLotteryGameServiceRunning = false;
            Utils.FileAppendThreadSafe(FileName, string.Format("Stop main thread at : {0}{1}{2}", DateTime.Now, Environment.NewLine, Environment.NewLine));
            Task.WaitAll(Tasks.ToArray());
        }

        // Get current BTC price
        private void ABC()
        {
            Utils.FileAppendThreadSafe(FileName, string.Format("Get current BTC thread on CPL window service STARTED on {0}{1}", DateTime.Now, Environment.NewLine));
            while (IsCPLLotteryGameServiceRunning)
            {
                try
                {
                    // TODO: Implement your code in here
                    var lotteries = Resolver.LotteryService
                        .Query()
                        .Include(x => x.LotteryHistories)
                        .Include(x => x.LotteryPrizes)
                        .Select()
                        .Where(x => x.Status.Equals((int)EnumLotteryGameStatus.ACTIVE) && x.Volume.Equals(x.LotteryHistories.Count))
                        .Select(x => Mapper.Map<LotteryModel>(x))
                        .ToList();

                    foreach (var lottery in lotteries)
                    {
                        var rawingTime = lottery.LotteryHistories.LastOrDefault().CreatedDate.Hour;
                    }

                    Thread.Sleep(RunningIntervalInMilliseconds);
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

        // random group all ticket to n group
        private void GroupTicket(ref List<LotteryHistory> lotteryHistories)
        {

        }

        // pick random 1 ticket from group to pick winner
        private void PickWinner()
        {

        }

        // AutoMapper Initialize
        private void AutoMapperInitialize()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<LotteryHistory, LotteryHistoryModel>();
                config.CreateMap<LotteryHistoryModel, LotteryHistory>();

                config.CreateMap<LotteryPrize, LotteryPrizeModel>();
                config.CreateMap<LotteryPrizeModel, LotteryPrize>();

                config.CreateMap<Lottery, LotteryModel>();
                config.CreateMap<LotteryModel, Lottery>();
            });
        }

        //Init dependency transaction & dbcontext
        private void Repository()
        {
            var builder = new ContainerBuilder();

            builder.Register(x =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<CPLContext>();
                optionsBuilder.UseSqlServer(ConnectionString);
                return optionsBuilder.Options;
            }).InstancePerLifetimeScope();

            builder.RegisterType<LotteryService>().As<ILotteryService>().InstancePerLifetimeScope();
            builder.RegisterType<LotteryHistoryService>().As<ILotteryHistoryService>().InstancePerLifetimeScope();
            builder.RegisterType<LotteryPrizeService>().As<ILotteryPrizeService>().InstancePerLifetimeScope();
            builder.RegisterType<SettingService>().As<ISettingService>().InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWorkAsync>().InstancePerLifetimeScope();
            builder.RegisterType<CPLContext>().As<IDataContextAsync>().InstancePerLifetimeScope();

            builder.RegisterType<Repository<Lottery>>().As<IRepositoryAsync<Lottery>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<LotteryHistory>>().As<IRepositoryAsync<LotteryHistory>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<LotteryPrize>>().As<IRepositoryAsync<LotteryPrize>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<Setting>>().As<IRepositoryAsync<Setting>>().InstancePerLifetimeScope();

            Resolver.Container = builder.Build();
            Resolver.UnitOfWork = Resolver.Container.Resolve<IUnitOfWorkAsync>();

            Resolver.LotteryService = Resolver.Container.Resolve<ILotteryService>();
            Resolver.LotteryHistoryService = Resolver.Container.Resolve<ILotteryHistoryService>();
            Resolver.LotteryPrizeService = Resolver.Container.Resolve<ILotteryPrizeService>();
            Resolver.SettingService = Resolver.Container.Resolve<ISettingService>();
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
            ConnectionString = Configuration["ConnectionString"];
        }
    }
}
