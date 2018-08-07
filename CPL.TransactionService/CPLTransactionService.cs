using Autofac;
using CPL.Common.Enums;
using CPL.Common.Misc;
using CPL.Core.Interfaces;
using CPL.Core.Services;
using CPL.Domain;
using CPL.Infrastructure;
using CPL.Infrastructure.Interfaces;
using CPL.Infrastructure.Repositories;
using CPL.TransactionService.Misc;
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
using System.Threading.Tasks;

namespace CPL.TransactionService
{
    public class CPLTransactionService : MicroService, IMicroService
    {
        public static IConfiguration Configuration { get; set; }
        public static bool IsTransactionServiceRunning = false;
        public static List<Task> Tasks = new List<Task>();

        public string FileName { get; set; }
        public string ConnectionString { get; set; }
        public int RunningIntervalInMilliseconds { get; set; }
        public string ServiceEnvironment { get; set; }

        public static AuthenticationService.AuthenticationClient _authentication = new AuthenticationService.AuthenticationClient();
        public static ETransactionService.ETransactionClient _eTransaction = new ETransactionService.ETransactionClient();
        public static BTransactionService.BTransactionClient _bTransaction = new BTransactionService.BTransactionClient();
        public static EAccountService.EAccountClient _eAccount = new EAccountService.EAccountClient();
        public static BAccountService.BAccountClient _bAccount = new BAccountService.BAccountClient();

        public struct Authentication
        {
            public static string Token { get; set; }
        }

        public readonly int NumberOfConfirmsForUnreversedTransaction = 6;

        public void Start()
        {
            // Configure Builder
            ConfigurationBuilder();

            // Initialize setting 
            InitializeSetting();

            // Initialize wcf 
            InitializeWCF();

            // write log
            Utils.FileAppendThreadSafe(FileName, String.Format("{0} started at {1}{2}", TransactionServiceConstant.ServiceName, DateTime.Now, Environment.NewLine));

            //Init dependency transaction & dbcontext
            InitializeRepositories();

            //Init setting
            IsTransactionServiceRunning = true;

            Tasks.Clear();
            Tasks.Add(Task.Run(() => CheckBTransaction()));
            //Tasks.Add(Task.Run(() => CheckETransaction()));
        }

        private void InitializeWCF()
        {
            var authentication = new AuthenticationService.AuthenticationClient().AuthenticateAsync(TransactionServiceConstant.Email, TransactionServiceConstant.ProjectName);
            authentication.Wait();

            if (authentication.Result.Status.Code == 0)
            {
                Authentication.Token = authentication.Result.Token;
                Utils.FileAppendThreadSafe(FileName, String.Format("Authenticate successfully. Token {0}{1}", authentication.Result.Token, Environment.NewLine));
                var bTransaction = _bTransaction.SetAsync(Authentication.Token, new BTransactionService.BTransactionSetting { Environment = (ServiceEnvironment == BTransactionService.Environment.MAINNET.ToString() ? BTransactionService.Environment.MAINNET : BTransactionService.Environment.TESTNET), Platform = BTransactionService.Platform.BTC});
                bTransaction.Wait();
                var eTransaction = _eTransaction.SetAsync(Authentication.Token, new ETransactionService.ETransactionSetting { Environment = (ServiceEnvironment == ETransactionService.Environment.MAINNET.ToString() ? ETransactionService.Environment.MAINNET : ETransactionService.Environment.TESTNET), Platform = ETransactionService.Platform.ETH, ApiKey= CPLConstant.ETransactionAPIKey });
                eTransaction.Wait();
            } else
            {
                Utils.FileAppendThreadSafe(FileName, String.Format("Authenticate failed. Error {0}{1}", authentication.Result.Status.Text, Environment.NewLine));
            }
        }

        private void CheckBTransaction()
        {
            var transactions = Resolver.BTCTransactionService.Queryable().Where(x => !x.UpdatedTime.HasValue);
            foreach(var transaction in transactions)
            {
                _bTransaction.RetrieveTransactionsAsync(Authentication.Token, transaction.a)
            }
        }

        public void Stop()
        {
            IsTransactionServiceRunning = false;
            Utils.FileAppendThreadSafe(FileName, String.Format("{0} stopped at {1}{2}", TransactionServiceConstant.ServiceName, DateTime.Now, Environment.NewLine));
            Task.WaitAll(Tasks.ToArray());
        }

        /// <summary>
        /// Initializes the repositories.
        /// </summary>
        private void InitializeRepositories()
        {
            var builder = new ContainerBuilder();

            builder.Register(x =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<CPLContext>();
                optionsBuilder.UseSqlServer(ConnectionString);
                return optionsBuilder.Options;
            }).InstancePerLifetimeScope();

            builder.RegisterType<SysUserService>().As<ISysUserService>().InstancePerLifetimeScope();
            builder.RegisterType<CoinTransactionService>().As<ICoinTransactionService>().InstancePerLifetimeScope();
            builder.RegisterType<BTCTransactionService>().As<IBTCTransactionService>().InstancePerLifetimeScope();
            builder.RegisterType<ETHTransactionService>().As<IETHTransactionService>().InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWorkAsync>().InstancePerLifetimeScope();
            builder.RegisterType<CPLContext>().As<IDataContextAsync>().InstancePerLifetimeScope();

            builder.RegisterType<Repository<CoinTransaction>>().As<IRepositoryAsync<CoinTransaction>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<SysUser>>().As<IRepositoryAsync<SysUser>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<BTCTransaction>>().As<IRepositoryAsync<BTCTransaction>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<ETHTransaction>>().As<IRepositoryAsync<ETHTransaction>>().InstancePerLifetimeScope();

            Resolver.Container = builder.Build();
            Resolver.UnitOfWork = Resolver.Container.Resolve<IUnitOfWorkAsync>();
            Resolver.SysUserService = Resolver.Container.Resolve<ISysUserService>();
            Resolver.CoinTransactionService = Resolver.Container.Resolve<ICoinTransactionService>();
            Resolver.BTCTransactionService = Resolver.Container.Resolve<IBTCTransactionService>();
            Resolver.ETHTransactionService = Resolver.Container.Resolve<IETHTransactionService>();
        }

        /// <summary>
        /// Configurations the builder.
        /// </summary>
        private void ConfigurationBuilder()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = configBuilder.Build();
        }

        /// <summary>
        /// Initializes the setting.
        /// </summary>
        private void InitializeSetting()
        {
            FileName = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "log.txt");
            RunningIntervalInMilliseconds = int.Parse(Configuration["RunningIntervalInMilliseconds"]);
            ConnectionString = Configuration["ConnectionString"];
            ServiceEnvironment = Configuration["Environment"];
        }
    }
}
