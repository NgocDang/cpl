using Autofac;
using CPL.Common.Enums;
using CPL.Common.Misc;
using CPL.Core.Interfaces;
using CPL.Core.Services;
using CPL.Domain;
using CPL.Infrastructure;
using CPL.Infrastructure.Interfaces;
using CPL.Infrastructure.Repositories;
using CPL.NotifyService.Misc;
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

namespace CPL.NotifyService
{
    public class CPLNotifyService : MicroService, IMicroService
    {
        public IConfiguration Configuration { get; set; }
        public bool IsNotifyServiceRunning = false;
        public List<Task> Tasks = new List<Task>();

        //public string ETHNotifyFileName { get; set; }
        public string BTCNotifyFileName { get; set; }

        public string ConnectionString { get; set; }
        //public int ETHRunningIntervalInMilliseconds { get; set; }
        public int BTCRunningIntervalInMilliseconds { get; set; }
        public string ServiceEnvironment { get; set; }

        public static AuthenticationService.AuthenticationClient _authentication = new AuthenticationService.AuthenticationClient();
        //public static EBlockService.EBlockClient _eBlock = new EBlockService.EBlockClient();
        //public static ETransactionService.ETransactionClient _eTransaction = new ETransactionService.ETransactionClient();
        public static BBlockService.BBlockClient _bBlock = new BBlockService.BBlockClient();

        public struct Authentication
        {
            public static string Token { get; set; }
        }

        public void Start()
        {
            // Configure Builder
            ConfigurationBuilder();

            // Initialize setting 
            InitializeSetting();

            // Initialize wcf 
            InitializeWCF();

            //Init setting
            IsNotifyServiceRunning = true;

            Tasks.Clear();
            //Tasks.Add(Task.Run(() => ETHNotify()));
            Tasks.Add(Task.Run(() => BTCNotify()));
        }

        private void InitializeWCF()
        {
            var authentication = new AuthenticationService.AuthenticationClient().AuthenticateAsync(CPLConstant.ProjectEmail, CPLConstant.ProjectName);
            authentication.Wait();

            if (authentication.Result.Status.Code == 0)
            {
                Authentication.Token = authentication.Result.Token;
                //Utils.FileAppendThreadSafe(ETHNotifyFileName, String.Format("ETH Nofity Thread - Authenticate successfully. Token {0}{1}", authentication.Result.Token, Environment.NewLine));
                Utils.FileAppendThreadSafe(BTCNotifyFileName, String.Format("BTC Nofity Thread - Authenticate successfully. Token {0}{1}", authentication.Result.Token, Environment.NewLine));
                //var eBlock = _eBlock.SetAsync(Authentication.Token, new EBlockService.EBlockSetting { Environment = (EBlockService.Environment)((int)CPLConstant.Environment) });
                //eBlock.Wait();
                var bBlock = _bBlock.SetAsync(Authentication.Token, new BBlockService.BBlockSetting { Environment = (BBlockService.Environment)((int)CPLConstant.Environment) });
                bBlock.Wait();

                //var eTransaction = _eTransaction.SetAsync(Authentication.Token, new ETransactionService.ETransactionSetting { Environment = (ETransactionService.Environment)((int)CPLConstant.Environment) });
                //eTransaction.Wait();
            }
            else
            {
                //Utils.FileAppendThreadSafe(ETHNotifyFileName, String.Format("ETH Nofity Thread - Authenticate failed. Error {0}{1}", authentication.Result.Status.Text, Environment.NewLine));
                Utils.FileAppendThreadSafe(BTCNotifyFileName, String.Format("BTC Nofity Thread - Authenticate failed. Error {0}{1}", authentication.Result.Status.Text, Environment.NewLine));
            }
        }

        //private void ETHNotify()
        //{
        //    do
        //    {
        //        try
        //        {
        //            //Init dependency transaction & dbcontext
        //            var resolver = InitializeRepositories();
        //            var currentBlockNoSetting = resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == NotifyServiceConstant.ETHNotifyCurrentBlockNo);
        //            var currentBlockNo = int.Parse(currentBlockNoSetting.Value);
        //            Task<EBlockService.EBlockRetrieveBlockTransactionResult> block;
        //            do
        //            {
        //                block = _eBlock.RetrieveBlockTransactionAsync(Authentication.Token, currentBlockNo, currentBlockNo);
        //                block.Wait();

        //                if (block.Result.Status.Code != 0)
        //                    Thread.Sleep(ETHRunningIntervalInMilliseconds);
        //            }
        //            while (IsNotifyServiceRunning && block.Result.Status.Code != 0);

        //            foreach (var tx in block.Result.Transactions)
        //            {
        //                if (resolver.SysUserService.Queryable().Any(x => x.ETHHDWalletAddress == tx.ToAddress))
        //                    resolver.ETHTransactionService.Insert(new ETHTransaction { CreatedDate = DateTime.Now, TxHashId = tx.TxHashId });
        //            }

        //            currentBlockNoSetting.Value = (++currentBlockNo).ToString();
        //            resolver.SettingService.Update(currentBlockNoSetting);
        //            resolver.UnitOfWork.SaveChanges();
        //        }
        //        catch (Exception ex)
        //        {
        //            if (ex.InnerException.Message != null)
        //                Utils.FileAppendThreadSafe(ETHNotifyFileName, string.Format("ETH Notify Thread - Exception {0} at {1}.{2}StackTrace {3}{4}", ex.InnerException.Message, DateTime.Now, Environment.NewLine, ex.StackTrace, Environment.NewLine));
        //            else
        //                Utils.FileAppendThreadSafe(ETHNotifyFileName, string.Format("ETH Notify Thread - Exception {0} at {1}.{2}StackTrace {3}{4}", ex.Message, DateTime.Now, Environment.NewLine, ex.StackTrace, Environment.NewLine));
        //        }
        //    }
        //    while (IsNotifyServiceRunning);
        //    Utils.FileAppendThreadSafe(ETHNotifyFileName, String.Format("ETH Notify Thread stopped at {1}{2}", NotifyServiceConstant.ServiceName, DateTime.Now, Environment.NewLine));
        //}

        private void BTCNotify()
        {
            do
            {
                try
                {
                    //Init dependency transaction & dbcontext
                    var resolver = InitializeRepositories();

                    var currentBlockNoSetting = resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == NotifyServiceConstant.BTCNotifyCurrentBlockNo);
                    var currentBlockNo = int.Parse(currentBlockNoSetting.Value);
                    Task<BBlockService.BBlockRetrieveBlockTransactionResult> block;
                    do
                    {
                        block = _bBlock.RetrieveBlockTransactionAsync(Authentication.Token, currentBlockNo, currentBlockNo);
                        block.Wait();

                        if (block.Result.Status.Code != 0)
                            Thread.Sleep(BTCRunningIntervalInMilliseconds);
                    }
                    while (IsNotifyServiceRunning && block.Result.Status.Code != 0);

                    foreach (var tx in block.Result.Transactions)
                    {
                        if (resolver.SysUserService.Queryable().Any(x => tx.To.Any(y => y.Address == x.BTCHDWalletAddress)))
                            resolver.BTCTransactionService.Insert(new BTCTransaction { CreatedDate = DateTime.Now, TxHashId = tx.TxHashId });
                    }

                    currentBlockNoSetting.Value = (++currentBlockNo).ToString();
                    resolver.SettingService.Update(currentBlockNoSetting);
                    resolver.UnitOfWork.SaveChanges();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.Message != null)
                        Utils.FileAppendThreadSafe(BTCNotifyFileName, string.Format("BTC Notify Thread - Exception {0} at {1}.{2}StackTrace {3}{4}", ex.InnerException.Message, DateTime.Now, Environment.NewLine, ex.StackTrace, Environment.NewLine));
                    else
                        Utils.FileAppendThreadSafe(BTCNotifyFileName, string.Format("BTC Notify Thread - Exception {0} at {1}.{2}StackTrace {3}{4}", ex.Message, DateTime.Now, Environment.NewLine, ex.StackTrace, Environment.NewLine));
                }
            }
            while (IsNotifyServiceRunning);
            Utils.FileAppendThreadSafe(BTCNotifyFileName, String.Format("BTC Notify Thread stopped at {1}{2}", NotifyServiceConstant.ServiceName, DateTime.Now, Environment.NewLine));
        }

        public void Stop()
        {
            IsNotifyServiceRunning = false;
            Task.WaitAll(Tasks.ToArray());
        }

        /// <summary>
        /// Initializes the repositories.
        /// </summary>
        private Resolver InitializeRepositories()
        {
            var builder = new ContainerBuilder();

            builder.Register(x =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<CPLContext>();
                optionsBuilder.UseSqlServer(ConnectionString);
                return optionsBuilder.Options;
            }).InstancePerLifetimeScope();

            builder.RegisterType<SysUserService>().As<ISysUserService>().InstancePerLifetimeScope();
            builder.RegisterType<SettingService>().As<ISettingService>().InstancePerLifetimeScope();
            builder.RegisterType<BTCTransactionService>().As<IBTCTransactionService>().InstancePerLifetimeScope();
            //builder.RegisterType<ETHTransactionService>().As<IETHTransactionService>().InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWorkAsync>().InstancePerLifetimeScope();
            builder.RegisterType<CPLContext>().As<IDataContextAsync>().InstancePerLifetimeScope();

            builder.RegisterType<Repository<Setting>>().As<IRepositoryAsync<Setting>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<SysUser>>().As<IRepositoryAsync<SysUser>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<BTCTransaction>>().As<IRepositoryAsync<BTCTransaction>>().InstancePerLifetimeScope();
            //builder.RegisterType<Repository<ETHTransaction>>().As<IRepositoryAsync<ETHTransaction>>().InstancePerLifetimeScope();

            var container = builder.Build();
            return new Resolver(
                container,
                container.Resolve<IUnitOfWorkAsync>(),
                container.Resolve<ISysUserService>(),
                container.Resolve<IBTCTransactionService>(),
                //container.Resolve<IETHTransactionService>(),
                container.Resolve<ISettingService>()
                );
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
            //ETHNotifyFileName = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "eth_notify_log.txt");
            BTCNotifyFileName = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "btc_notify_log.txt");

            //ETHRunningIntervalInMilliseconds = int.Parse(Configuration["ETHRunningIntervalInMilliseconds"]);
            BTCRunningIntervalInMilliseconds = int.Parse(Configuration["BTCRunningIntervalInMilliseconds"]);
            ConnectionString = Configuration["ConnectionString"];
            ServiceEnvironment = Configuration["Environment"];
        }
    }
}
