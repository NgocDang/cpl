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
using System.Threading;
using System.Threading.Tasks;

namespace CPL.TransactionService
{
    public class CPLTransactionService : MicroService, IMicroService
    {
        public static IConfiguration Configuration { get; set; }
        public static bool IsTransactionServiceRunning = false;
        public static List<Task> Tasks = new List<Task>();

        public string BTCFileName { get; set; }
        public string ETHFileName { get; set; }
        public string ConnectionString { get; set; }
        public int RunningIntervalInMilliseconds { get; set; }
        public string ServiceEnvironment { get; set; }
        public int NumberOfConfirmsForUnreversedBTCTransaction { get; set; }

        public static AuthenticationService.AuthenticationClient _authentication = new AuthenticationService.AuthenticationClient();
        public static ETransactionService.ETransactionClient _eTransaction = new ETransactionService.ETransactionClient();
        public static BTransactionService.BTransactionClient _bTransaction = new BTransactionService.BTransactionClient();
        public static EAccountService.EAccountClient _eAccount = new EAccountService.EAccountClient();
        public static BAccountService.BAccountClient _bAccount = new BAccountService.BAccountClient();

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
                Utils.FileAppendThreadSafe(BTCFileName, String.Format("BTC Thread - Authenticate successfully. Token {0}{1}", authentication.Result.Token, Environment.NewLine));
                var bTransaction = _bTransaction.SetAsync(Authentication.Token, new BTransactionService.BTransactionSetting { Environment = (ServiceEnvironment == BTransactionService.Environment.MAINNET.ToString() ? BTransactionService.Environment.MAINNET : BTransactionService.Environment.TESTNET), Platform = BTransactionService.Platform.BTC});
                bTransaction.Wait();
                var eTransaction = _eTransaction.SetAsync(Authentication.Token, new ETransactionService.ETransactionSetting { Environment = (ServiceEnvironment == ETransactionService.Environment.MAINNET.ToString() ? ETransactionService.Environment.MAINNET : ETransactionService.Environment.TESTNET), Platform = ETransactionService.Platform.ETH, ApiKey= CPLConstant.ETransactionAPIKey });
                eTransaction.Wait();
            } else
            {
                Utils.FileAppendThreadSafe(BTCFileName, String.Format("BTC Thread - Authenticate failed. Error {0}{1}", authentication.Result.Status.Text, Environment.NewLine));
            }
        }

        private void CheckBTransaction()
        {
            try
            {
                do
                {
                    var transactions = new List<BTCTransaction>();
                    do
                    {
                        transactions = Resolver.BTCTransactionService.Queryable().Where(x => !x.UpdatedTime.HasValue).ToList();
                        if (transactions.Count == 0)
                            Thread.Sleep(RunningIntervalInMilliseconds);
                    }
                    while (IsTransactionServiceRunning && transactions.Count == 0);

                    Utils.FileAppendThreadSafe(BTCFileName, String.Format("BTC Thread - Number of transactions {0} need to be checked.{1}", transactions.Count, Environment.NewLine));

                    foreach (var transaction in transactions)
                    {
                        var transactionDetail = _bTransaction.RetrieveTransactionDetailAsync(Authentication.Token, transaction.TxHashId);
                        transactionDetail.Wait();

                        if (transactionDetail.Result.Confirmations >= NumberOfConfirmsForUnreversedBTCTransaction)
                        {
                            var user = Resolver.SysUserService.Queryable()
                                .FirstOrDefault(x => transactionDetail.Result.To.Select(y => y.Address).Contains(x.BTCHDWalletAddress));
                            if (user != null)
                            {
                                // add BTC amount
                                user.BTCAmount += transactionDetail.Result.Value;
                                Resolver.SysUserService.Update(user);

                                // update btc transaction so that it is not checked next time
                                transaction.UpdatedTime = DateTime.Now;
                                Resolver.BTCTransactionService.Update(transaction);

                                // add record to coin transaction
                                Resolver.CoinTransactionService.Insert(new CoinTransaction
                                {
                                    CoinAmount = transactionDetail.Result.Value,
                                    CreatedDate = DateTime.Now,
                                    CurrencyId = (int)EnumCurrency.BTC,
                                    SysUserId = user.Id,
                                    ToWalletAddress = user.BTCHDWalletAddress,
                                    TxHashId = transaction.TxHashId,
                                    Type = (int)EnumCoinTransactionType.DEPOSIT_BTC
                                });
                            }
                        }
                    }

                    Resolver.UnitOfWork.SaveChanges();
                }
                while (IsTransactionServiceRunning);
                
            }
            catch(Exception ex)
            {
                if (ex.InnerException.Message != null)
                    Utils.FileAppendThreadSafe(BTCFileName, string.Format("BTC Thread - Exception {0} at {1}{2}", ex.InnerException.Message, DateTime.Now, Environment.NewLine));
                else
                    Utils.FileAppendThreadSafe(BTCFileName, string.Format("BTC Thread - Exception {0} at {1}{2}", ex.Message, DateTime.Now, Environment.NewLine));
            }
            Utils.FileAppendThreadSafe(BTCFileName, String.Format("BTC Thread stopped at {1}{2}", TransactionServiceConstant.ServiceName, DateTime.Now, Environment.NewLine));
        }

        private void CheckETransaction()
        {
            try
            {
                do
                {
                    var transactions = new List<ETHTransaction>();
                    do
                    {
                        transactions = Resolver.ETHTransactionService.Queryable().Where(x => !x.UpdatedTime.HasValue).ToList();
                        if (transactions.Count == 0)
                            Thread.Sleep(RunningIntervalInMilliseconds);
                    }
                    while (IsTransactionServiceRunning && transactions.Count == 0);

                    Utils.FileAppendThreadSafe(BTCFileName, String.Format("ETH Thread - Number of transactions {0} need to be checked.{1}", transactions.Count, Environment.NewLine));

                    foreach (var transaction in transactions)
                    {
                        var transactionDetail = _eTransaction.RetrieveTransactionDetailAsync(Authentication.Token, transaction.TxHashId);
                        transactionDetail.Wait();

                        if (transactionDetail.Result.TransactionStatus.HasValue && transactionDetail.Result.TransactionStatus.Value)
                        {
                            var user = Resolver.SysUserService.Queryable()
                                .FirstOrDefault(x => x.ETHHDWalletAddress == transactionDetail.Result.ToAddress);
                            if (user != null)
                            {
                                // add BTC amount
                                user.ETHAmount += transactionDetail.Result.Value;
                                Resolver.SysUserService.Update(user);

                                // update btc transaction so that it is not checked next time
                                transaction.UpdatedTime = DateTime.Now;
                                Resolver.ETHTransactionService.Update(transaction);

                                // add record to coin transaction
                                Resolver.CoinTransactionService.Insert(new CoinTransaction
                                {
                                    CoinAmount = transactionDetail.Result.Value,
                                    CreatedDate = DateTime.Now,
                                    CurrencyId = (int)EnumCurrency.ETH,
                                    SysUserId = user.Id,
                                    ToWalletAddress = user.ETHHDWalletAddress,
                                    TxHashId = transaction.TxHashId,
                                    Type = (int)EnumCoinTransactionType.DEPOSIT_ETH
                                });
                            }
                        }
                    }
                    Resolver.UnitOfWork.SaveChanges();
                }
                while (IsTransactionServiceRunning);

            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message != null)
                    Utils.FileAppendThreadSafe(ETHFileName, string.Format("ETH Thread - Exception {0} at {1}{2}", ex.InnerException.Message, DateTime.Now, Environment.NewLine));
                else
                    Utils.FileAppendThreadSafe(ETHFileName, string.Format("ETH Thread - Exception {0} at {1}{2}", ex.Message, DateTime.Now, Environment.NewLine));
            }
            Utils.FileAppendThreadSafe(ETHFileName, String.Format("ETH Thread stopped at {1}{2}", TransactionServiceConstant.ServiceName, DateTime.Now, Environment.NewLine));
        }

        public void Stop()
        {
            IsTransactionServiceRunning = false;
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
            BTCFileName = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "btc_log.txt");
            ETHFileName = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "eth_log.txt");
            RunningIntervalInMilliseconds = int.Parse(Configuration["RunningIntervalInMilliseconds"]);
            NumberOfConfirmsForUnreversedBTCTransaction = int.Parse(Configuration["NumberOfConfirmsForUnreversedBTCTransaction"]);
            ConnectionString = Configuration["ConnectionString"];
            ServiceEnvironment = Configuration["Environment"];
        }
    }
}
