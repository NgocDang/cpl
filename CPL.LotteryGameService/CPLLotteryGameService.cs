using Autofac;
using CPL.Infrastructure;
using CPL.Infrastructure.Interfaces;
using CPL.Infrastructure.Repositories;
using CPL.LotteryGameService.Misc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using PeterKottas.DotNetCore.WindowsService.Base;
using PeterKottas.DotNetCore.WindowsService.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
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

            //Init setting
            IsCPLLotteryGameServiceRunning = true;
            Tasks.Clear();
            Tasks.Add(Task.Run(() => ABC()));
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


            builder.RegisterType<UnitOfWork>().As<IUnitOfWorkAsync>().InstancePerLifetimeScope();
            builder.RegisterType<CPLContext>().As<IDataContextAsync>().InstancePerLifetimeScope();


            Resolver.Container = builder.Build();
            Resolver.UnitOfWork = Resolver.Container.Resolve<IUnitOfWorkAsync>();
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
