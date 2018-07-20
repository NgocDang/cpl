using Autofac;
using BTCCurrentPriceService;
using CPL.Core.Interfaces;
using CPL.Core.Services;
using CPL.Domain;
using CPL.Infrastructure;
using CPL.Infrastructure.Interfaces;
using CPL.Infrastructure.Repositories;
using CPL.WindowService.Misc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using PeterKottas.DotNetCore.WindowsService.Base;
using PeterKottas.DotNetCore.WindowsService.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CPL.WindowService
{
    public class CPLWindowService : MicroService, IMicroService
    {
        public static IConfiguration Configuration { get; set; }
        public static BTCCurrentPriceClient BTCCurrentPriceClient = new BTCCurrentPriceClient();
        public static bool IsCPLWindowServiceRunning = false;
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
            IsCPLWindowServiceRunning = true;
            Tasks.Clear();
            Tasks.Add(Task.Run(() => GetCurrentBTCPrice()));
        }

        public void Stop()
        {
            IsCPLWindowServiceRunning = false;
            Utils.FileAppendThreadSafe(FileName, string.Format("Stop main thread at : {0}{1}{2}", DateTime.Now, Environment.NewLine, Environment.NewLine));
            Task.WaitAll(Tasks.ToArray());
        }

        // Get current BTC price
        private void GetCurrentBTCPrice()
        {
            Utils.FileAppendThreadSafe(FileName, string.Format("Get current BTC thread on CPL window service STARTED on {0}{1}", DateTime.Now, Environment.NewLine));
            while (IsCPLWindowServiceRunning)
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

                    Resolver.BTCPriceService.Insert(btcPrice);
                    Resolver.UnitOfWork.SaveChanges();

                    Thread.Sleep(RunningIntervalInMilliseconds);
                }
                catch(Exception ex)
                {
                    if(ex.InnerException.Message != null)
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

            builder.RegisterType<BTCPriceService>().As<IBTCPriceService>().InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWorkAsync>().InstancePerLifetimeScope();
            builder.RegisterType<CPLContext>().As<IDataContextAsync>().InstancePerLifetimeScope();

            builder.RegisterType<Repository<BTCPrice>>().As<IRepositoryAsync<BTCPrice>>().InstancePerLifetimeScope();

            Resolver.Container = builder.Build();
            Resolver.BTCPriceService = Resolver.Container.Resolve<IBTCPriceService>();
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
            BTCCurrentPriceClient.Endpoint.Address = new EndpointAddress(new Uri(Configuration["BTCCurrentPriceClientEndpoint"]));
        }
    }
}
