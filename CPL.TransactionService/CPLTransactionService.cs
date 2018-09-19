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
using Microsoft.Extensions.DependencyInjection;
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
        public IConfiguration Configuration { get; set; }
        public bool IsTransactionServiceRunning = false;
        public List<Task> Tasks = new List<Task>();

        public string BTCDepositFileName { get; set; }
        //public string ETHDepositFileName { get; set; }
        public string BTCWithdrawFileName { get; set; }
        //public string ETHWithdrawFileName { get; set; }

        public string ConnectionString { get; set; }
        public int RunningIntervalInMilliseconds { get; set; }
        public int NumberOfConfirmsForUnreversedBTCTransaction { get; set; }
        public int NumberOfDaysFailTransaction { get; set; }

        public static AuthenticationService.AuthenticationClient _authentication = new AuthenticationService.AuthenticationClient();
        //public static ETransactionService.ETransactionClient _eTransaction = new ETransactionService.ETransactionClient();
        public static BTransactionService.BTransactionClient _bTransaction = new BTransactionService.BTransactionClient();
        //public static EAccountService.EAccountClient _eAccount = new EAccountService.EAccountClient();
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

            //Init setting
            IsTransactionServiceRunning = true;

            Tasks.Clear();
            Tasks.Add(Task.Run(() => DepositBTransactionAsync()));
            //Tasks.Add(Task.Run(() => DepositETransactionAsync()));
            Tasks.Add(Task.Run(() => WithdrawBTransactionAsync()));
            //Tasks.Add(Task.Run(() => WithdrawETransactionAsync()));
        }

        private void InitializeWCF()
        {
            var authentication = new AuthenticationService.AuthenticationClient().AuthenticateAsync(CPLConstant.ProjectEmail, CPLConstant.ProjectName);
            authentication.Wait();

            if (authentication.Result.Status.Code == 0)
            {
                Authentication.Token = authentication.Result.Token;
                Utils.FileAppendThreadSafe(BTCDepositFileName, String.Format("BTC Deposit Thread - Authenticate successfully. Token {0}{1}", authentication.Result.Token, Environment.NewLine));
                //Utils.FileAppendThreadSafe(ETHDepositFileName, String.Format("ETH Deposit Thread - Authenticate successfully. Token {0}{1}", authentication.Result.Token, Environment.NewLine));
                Utils.FileAppendThreadSafe(BTCWithdrawFileName, String.Format("BTC Withdraw Thread - Authenticate successfully. Token {0}{1}", authentication.Result.Token, Environment.NewLine));
                //Utils.FileAppendThreadSafe(ETHWithdrawFileName, String.Format("ETH Withdraw Thread - Authenticate successfully. Token {0}{1}", authentication.Result.Token, Environment.NewLine));
                var bTransaction = _bTransaction.SetAsync(Authentication.Token, new BTransactionService.BTransactionSetting { Environment = (BTransactionService.Environment)((int)CPLConstant.Environment), Platform = BTransactionService.Platform.BTC });
                bTransaction.Wait();
                //var eTransaction = _eTransaction.SetAsync(Authentication.Token, new ETransactionService.ETransactionSetting { Environment = (ETransactionService.Environment)((int)CPLConstant.Environment), Platform = ETransactionService.Platform.ETH, ApiKey = CPLConstant.ETransactionAPIKey });
                //eTransaction.Wait();
            }
            else
            {
                Utils.FileAppendThreadSafe(BTCDepositFileName, String.Format("BTC Deposit Thread - Authenticate failed. Error {0}{1}", authentication.Result.Status.Text, Environment.NewLine));
                //Utils.FileAppendThreadSafe(ETHDepositFileName, String.Format("ETH Deposit Thread - Authenticate failed. Error {0}{1}", authentication.Result.Status.Text, Environment.NewLine));
                Utils.FileAppendThreadSafe(BTCWithdrawFileName, String.Format("BTC Withdraw Thread - Authenticate failed. Error {0}{1}", authentication.Result.Status.Text, Environment.NewLine));
                //Utils.FileAppendThreadSafe(ETHWithdrawFileName, String.Format("ETH Withdraw Thread - Authenticate failed. Error {0}{1}", authentication.Result.Status.Text, Environment.NewLine));
            }
        }

        private async void DepositBTransactionAsync()
        {
            do
            {
                try
                {

                    //Init dependency transaction & dbcontext
                    var resolver = InitializeRepositories();
                    var transactions = new List<BTCTransaction>();
                    do
                    {
                        transactions = resolver.BTCTransactionService.Queryable().Where(x => !x.UpdatedTime.HasValue).ToList();
                        if (transactions.Count == 0)
                            await Task.Delay(RunningIntervalInMilliseconds);
                    }
                    while (IsTransactionServiceRunning && transactions.Count == 0);

                    Utils.FileAppendThreadSafe(BTCDepositFileName, String.Format("BTC Deposit Thread - Number of transactions {0} need to be checked.{1}", transactions.Count, Environment.NewLine));

                    foreach (var transaction in transactions)
                    {
                        var transactionDetail = _bTransaction.RetrieveTransactionDetailAsync(Authentication.Token, transaction.TxHashId);
                        transactionDetail.Wait();

                        if (transactionDetail == null)
                        {
                            var diff = DateTime.Now - transaction.CreatedDate;
                            if (diff.Days >= NumberOfDaysFailTransaction)
                            {
                                // update record to eth transaction
                                transaction.UpdatedTime = DateTime.Now;
                                transaction.Status = false;
                                resolver.BTCTransactionService.Update(transaction);

                                // Record log in case of internal transfer error
                                if (transaction.ParentId.HasValue)
                                {
                                    Utils.FileAppendThreadSafe(BTCDepositFileName, string.Format("BTC Deposit Thread - There was a error with txHashId {0}. So, user cannot receive the money at {1}{2}", transaction.TxHashId, DateTime.Now, Environment.NewLine));
                                }
                            }
                        }
                        else if (transactionDetail.Result.Confirmations >= NumberOfConfirmsForUnreversedBTCTransaction)
                        {
                            // user transfer
                            if (!transaction.ParentId.HasValue)
                            {
                                var user = resolver.SysUserService.Queryable()
                                        .FirstOrDefault(x => transactionDetail.Result.To.Select(y => y.Address).Contains(x.BTCHDWalletAddress));
                                if (user != null)
                                {
                                    // update btc transaction so that it is not checked next time
                                    transaction.UpdatedTime = DateTime.Now;
                                    transaction.Status = true;
                                    resolver.BTCTransactionService.Update(transaction);

                                    // do internal transfer
                                    var transactionToDepositAddress = _bAccount.TransferByMnemonicAsync(Authentication.Token, CPLConstant.BTCMnemonic, user.BTCHDWalletAddressIndex.ToString(), CPLConstant.BTCDepositAddress, CPLConstant.DurationInSecond);
                                    transactionToDepositAddress.Wait();
                                    if (transactionToDepositAddress.Result.Status.Code == 0)
                                    {
                                        resolver.BTCTransactionService.Insert(new BTCTransaction
                                        {
                                            TxHashId = transactionToDepositAddress.Result.TxId.FirstOrDefault(),
                                            CreatedDate = DateTime.Now,
                                            ParentId = transaction.Id
                                        });
                                    }
                                }
                                else // inserted by bitcoinD
                                {
                                    // To be removed after implement bBlock
                                    // update record to ignored check this transaction by the service
                                    transaction.UpdatedTime = DateTime.Now;
                                    transaction.Status = false;
                                    resolver.BTCTransactionService.Update(transaction);
                                }
                            }
                            // internal transfer
                            else
                            {
                                // find transaction parrent
                                var transactionParent = resolver.BTCTransactionService.Queryable().Where(x => x.Id == transaction.ParentId).FirstOrDefault();
                                var transactionParentDetail = _bTransaction.RetrieveTransactionDetailAsync(Authentication.Token, transactionParent.TxHashId);
                                transactionParentDetail.Wait();

                                if (transactionParentDetail.Result.Status.Code == 0)
                                {
                                    // find user to send money
                                    var user = resolver.SysUserService.Queryable()
                                        .FirstOrDefault(x => transactionParentDetail.Result.To.Select(y => y.Address).Contains(x.BTCHDWalletAddress));

                                    // update BTC amount
                                    var coinAmount = transactionParentDetail.Result.To.FirstOrDefault(x => x.Address == user.BTCHDWalletAddress).Value;
                                    user.BTCAmount += coinAmount;
                                    resolver.SysUserService.Update(user);

                                    // add record to coin transaction of user transfer (not internal transfer)
                                    resolver.CoinTransactionService.Insert(new CoinTransaction
                                    {
                                        CoinAmount = coinAmount,
                                        CreatedDate = DateTime.Now,
                                        CurrencyId = (int)EnumCurrency.BTC,
                                        SysUserId = user.Id,
                                        ToWalletAddress = user.BTCHDWalletAddress,
                                        TxHashId = transactionParentDetail.Result.TxHashId,
                                        Status = true,
                                        Type = (int)EnumCoinTransactionType.DEPOSIT_BTC
                                    });

                                    // update btc transaction so that it is not checked next time
                                    transaction.UpdatedTime = DateTime.Now;
                                    transaction.Status = true;
                                    resolver.BTCTransactionService.Update(transaction);
                                }
                            }
                        }
                    }
                    resolver.UnitOfWork.SaveChanges();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message != null)
                        Utils.FileAppendThreadSafe(BTCDepositFileName, string.Format("BTC Deposit Thread - Exception {0} at {1}.{2}StackTrace {3}{4}", ex.InnerException.Message, DateTime.Now, Environment.NewLine, ex.StackTrace, Environment.NewLine));
                    else
                        Utils.FileAppendThreadSafe(BTCDepositFileName, string.Format("BTC Deposit Thread - Exception {0} at {1}.{2}StackTrace {3}{4}", ex.Message, DateTime.Now, Environment.NewLine, ex.StackTrace, Environment.NewLine));
                }
            }
            while (IsTransactionServiceRunning);
            Utils.FileAppendThreadSafe(BTCDepositFileName, String.Format("BTC Deposit Thread stopped at {1}{2}", TransactionServiceConstant.ServiceName, DateTime.Now, Environment.NewLine));
        }

        //private async void DepositETransactionAsync()
        //{
        //    do
        //    {
        //        try
        //        {

        //            //Init dependency transaction & dbcontext
        //            var resolver = InitializeRepositories();
        //            var transactions = new List<ETHTransaction>();
        //            do
        //            {
        //                transactions = resolver.ETHTransactionService.Queryable().Where(x => !x.UpdatedTime.HasValue).ToList();
        //                if (transactions.Count == 0)
        //                    await Task.Delay(RunningIntervalInMilliseconds);
        //            }
        //            while (IsTransactionServiceRunning && transactions.Count == 0);

        //            Utils.FileAppendThreadSafe(ETHDepositFileName, String.Format("ETH Deposit Thread - Number of transactions {0} need to be checked.{1}", transactions.Count, Environment.NewLine));

        //            foreach (var transaction in transactions)
        //            {
        //                var transactionDetail = _eTransaction.RetrieveTransactionDetailAsync(Authentication.Token, transaction.TxHashId);
        //                transactionDetail.Wait();

        //                if (transactionDetail == null || transactionDetail.Result.TransactionStatus == null || !transactionDetail.Result.TransactionStatus.Value)
        //                {
        //                    var diff = DateTime.Now - transaction.CreatedDate;
        //                    if (diff.Days >= NumberOfDaysFailTransaction)
        //                    {
        //                        // update record to eth transaction
        //                        transaction.UpdatedTime = DateTime.Now;
        //                        transaction.Status = false;
        //                        resolver.ETHTransactionService.Update(transaction);

        //                        // Record log in case of internal transfer error
        //                        if (transaction.ParentId.HasValue)
        //                        {
        //                            Utils.FileAppendThreadSafe(ETHDepositFileName, string.Format("ETH Deposit Thread - There was a error with txHashId {0}. So, user cannot receive the money at {1}{2}", transaction.TxHashId, DateTime.Now, Environment.NewLine));
        //                        }
        //                    }
        //                }
        //                else if (transactionDetail.Result.TransactionStatus.HasValue && transactionDetail.Result.TransactionStatus.Value)
        //                {
        //                    // user transfer
        //                    if (!transaction.ParentId.HasValue)
        //                    {
        //                        var user = resolver.SysUserService.Queryable()
        //                            .FirstOrDefault(x => x.ETHHDWalletAddress == transactionDetail.Result.ToAddress);
        //                        if (user != null)
        //                        {
        //                            // update eth transaction so that it is not checked next time
        //                            transaction.UpdatedTime = DateTime.Now;
        //                            transaction.Status = true;
        //                            resolver.ETHTransactionService.Update(transaction);
        //                            // do internal transfer
        //                            var transactionToDepositAddress = _eAccount.TransferByMnemonicAsync(Authentication.Token, CPLConstant.ETHMnemonic, user.ETHHDWalletAddressIndex.ToString(), CPLConstant.ETHWithdrawAddress, CPLConstant.DurationInSecond);
        //                            transactionToDepositAddress.Wait();
        //                            if (transactionToDepositAddress.Result.Status.Code == 0)
        //                            {
        //                                resolver.ETHTransactionService.Insert(new ETHTransaction
        //                                {
        //                                    TxHashId = transactionToDepositAddress.Result.TxId.FirstOrDefault(),
        //                                    CreatedDate = DateTime.Now,
        //                                    ParentId = transaction.Id
        //                                });
        //                            }
        //                        }
        //                        else
        //                        {
        //                            // This case will never be happened
        //                        }
        //                    }
        //                    // internal transfer
        //                    else
        //                    {
        //                        // find transaction parrent
        //                        var transactionParent = resolver.ETHTransactionService.Queryable().Where(x => x.Id == transaction.ParentId).FirstOrDefault();
        //                        var transactionParentDetail = _eTransaction.RetrieveTransactionDetailAsync(Authentication.Token, transactionParent.TxHashId);
        //                        transactionParentDetail.Wait();

        //                        if (transactionParentDetail.Result.Status.Code == 0)
        //                        {
        //                            // find user to send money
        //                            var user = resolver.SysUserService.Queryable()
        //                                .FirstOrDefault(x => x.ETHHDWalletAddress == transactionParentDetail.Result.ToAddress);

        //                            // update ETH amount
        //                            var coinAmount = transactionParentDetail.Result.Value;
        //                            user.ETHAmount += coinAmount;
        //                            resolver.SysUserService.Update(user);

        //                            // add record to coin transaction of user transfer (not internal transfer)
        //                            resolver.CoinTransactionService.Insert(new CoinTransaction
        //                            {
        //                                CoinAmount = coinAmount,
        //                                CreatedDate = DateTime.Now,
        //                                CurrencyId = (int)EnumCurrency.ETH,
        //                                SysUserId = user.Id,
        //                                ToWalletAddress = user.ETHHDWalletAddress,
        //                                TxHashId = transactionParentDetail.Result.TxHashId,
        //                                Status = true,
        //                                Type = (int)EnumCoinTransactionType.DEPOSIT_ETH
        //                            });

        //                            // update eth transaction so that it is not checked next time
        //                            transaction.UpdatedTime = DateTime.Now;
        //                            transaction.Status = true;
        //                            resolver.ETHTransactionService.Update(transaction);
        //                        }
        //                    }
        //                }
        //            }
        //            resolver.UnitOfWork.SaveChanges();
        //        }
        //        catch (Exception ex)
        //        {
        //            if (ex.InnerException != null && ex.InnerException.Message != null)
        //                Utils.FileAppendThreadSafe(ETHDepositFileName, string.Format("ETH Deposit Thread - Exception {0} at {1}.{2}StackTrace {3}{4}", ex.InnerException.Message, DateTime.Now, Environment.NewLine, ex.StackTrace, Environment.NewLine));
        //            else
        //                Utils.FileAppendThreadSafe(ETHDepositFileName, string.Format("ETH Deposit Thread - Exception {0} at {1}.{2}StackTrace {3}{4}", ex.Message, DateTime.Now, Environment.NewLine, ex.StackTrace, Environment.NewLine));

        //        }

        //    }
        //    while (IsTransactionServiceRunning);
        //    Utils.FileAppendThreadSafe(ETHDepositFileName, String.Format("ETH Deposit Thread stopped at {1}{2}", TransactionServiceConstant.ServiceName, DateTime.Now, Environment.NewLine));
        //}

        private async void WithdrawBTransactionAsync()
        {
            do
            {
                try
                {
                    //Init dependency transaction & dbcontext
                    var resolver = InitializeRepositories();
                    var btcToTokenRate = decimal.Parse(resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.BTCToTokenRate).Value);

                    var transactions = new List<CoinTransaction>();
                    do
                    {
                        transactions = resolver.CoinTransactionService.Queryable().Where(x => !x.Status.HasValue
                        && x.Type == (int)EnumCoinTransactionType.WITHDRAW_BTC
                        && x.CurrencyId == (int)EnumCurrency.BTC).ToList();
                        if (transactions.Count == 0)
                            await Task.Delay(RunningIntervalInMilliseconds);
                    }
                    while (IsTransactionServiceRunning && transactions.Count == 0);

                    Utils.FileAppendThreadSafe(BTCWithdrawFileName, String.Format("BTC Withdraw Thread - Number of transactions {0} need to be checked.{1}", transactions.Count, Environment.NewLine));

                    foreach (var transaction in transactions)
                    {
                        var transactionDetail = _bTransaction.RetrieveTransactionDetailAsync(Authentication.Token, transaction.TxHashId);
                        transactionDetail.Wait();

                        if (transactionDetail == null)
                        {
                            var diff = DateTime.Now - transaction.CreatedDate;
                            if (diff.Days >= NumberOfDaysFailTransaction)
                            {
                                // update record to coin transaction
                                transaction.Status = false;
                                resolver.CoinTransactionService.Update(transaction);

                                // update record to sysuser
                                var user = resolver.SysUserService.Queryable()
                                    .FirstOrDefault(x => x.Id == transaction.SysUserId);
                                user.TokenAmount += transaction.CoinAmount * btcToTokenRate;
                                resolver.SysUserService.Update(user);
                            }
                        }
                        else if (transactionDetail.Result.Confirmations >= 1)
                        {
                            // update record to coin transaction
                            transaction.Status = true;
                            resolver.CoinTransactionService.Update(transaction);
                        }
                    }
                    resolver.UnitOfWork.SaveChanges();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message != null)
                        Utils.FileAppendThreadSafe(BTCWithdrawFileName, string.Format("BTC Withdraw Thread - Exception {0} at {1}.{2}StackTrace {3}{4}", ex.InnerException.Message, DateTime.Now, Environment.NewLine, ex.StackTrace, Environment.NewLine));
                    else
                        Utils.FileAppendThreadSafe(BTCWithdrawFileName, string.Format("BTC Withdraw Thread - Exception {0} at {1}.{2}StackTrace {3}{4}", ex.Message, DateTime.Now, Environment.NewLine, ex.StackTrace, Environment.NewLine));
                }
            }
            while (IsTransactionServiceRunning);

            Utils.FileAppendThreadSafe(BTCWithdrawFileName, String.Format("BTC Withdraw Thread stopped at {1}{2}", TransactionServiceConstant.ServiceName, DateTime.Now, Environment.NewLine));
        }

        //private async void WithdrawETransactionAsync()
        //{
        //    do
        //    {
        //        try
        //        {

        //            //Init dependency transaction & dbcontext
        //            var resolver = InitializeRepositories();
        //            var transactions = new List<CoinTransaction>();
        //            do
        //            {
        //                transactions = resolver.CoinTransactionService.Queryable().Where(x => !x.Status.HasValue
        //                && x.Type == (int)EnumCoinTransactionType.WITHDRAW_ETH
        //                && x.CurrencyId == (int)EnumCurrency.ETH).ToList();
        //                if (transactions.Count == 0)
        //                    await Task.Delay(RunningIntervalInMilliseconds);
        //            }
        //            while (IsTransactionServiceRunning && transactions.Count == 0);

        //            Utils.FileAppendThreadSafe(ETHWithdrawFileName, String.Format("ETH Withdraw Thread - Number of transactions {0} need to be checked.{1}", transactions.Count, Environment.NewLine));

        //            foreach (var transaction in transactions)
        //            {
        //                var transactionStatus = _eTransaction.GetTransactionStatusAsync(Authentication.Token, transaction.TxHashId);
        //                transactionStatus.Wait();

        //                if (transactionStatus.Result.Receipt.HasValue)
        //                {
        //                    if (transactionStatus.Result.Receipt.Value)
        //                    {
        //                        // update record to coin transaction
        //                        transaction.Status = true;
        //                        resolver.CoinTransactionService.Update(transaction);
        //                    }
        //                    else
        //                    {
        //                        var diff = DateTime.Now - transaction.CreatedDate;
        //                        if (diff.Days >= NumberOfDaysFailTransaction)
        //                        {
        //                            // update record to coin transaction
        //                            transaction.Status = false;
        //                            resolver.CoinTransactionService.Update(transaction);

        //                            // update record to sysuser
        //                            var user = resolver.SysUserService.Queryable()
        //                                .FirstOrDefault(x => x.Id == transaction.SysUserId);
        //                            user.ETHAmount += transaction.CoinAmount;
        //                            resolver.SysUserService.Update(user);
        //                        }
        //                    }
        //                }
        //            }

        //            resolver.UnitOfWork.SaveChanges();
        //        }
        //        catch (Exception ex)
        //        {
        //            if (ex.InnerException != null && ex.InnerException.Message != null)
        //                Utils.FileAppendThreadSafe(ETHWithdrawFileName, string.Format("ETH Withdraw Thread - Exception {0} at {1}.{2}StackTrace {3}{4}", ex.InnerException.Message, DateTime.Now, Environment.NewLine, ex.StackTrace, Environment.NewLine));
        //            else
        //                Utils.FileAppendThreadSafe(ETHWithdrawFileName, string.Format("ETH Withdraw Thread - Exception {0} at {1}.{2}StackTrace {3}{4}", ex.Message, DateTime.Now, Environment.NewLine, ex.StackTrace, Environment.NewLine));
        //        }

        //    }
        //    while (IsTransactionServiceRunning);
        //    Utils.FileAppendThreadSafe(ETHWithdrawFileName, String.Format("ETH Withdraw Thread stopped at {1}{2}", TransactionServiceConstant.ServiceName, DateTime.Now, Environment.NewLine));
        //}

        public void Stop()
        {
            IsTransactionServiceRunning = false;
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
            });

            builder.RegisterType<SysUserService>().As<ISysUserService>().InstancePerDependency();
            builder.RegisterType<CoinTransactionService>().As<ICoinTransactionService>().InstancePerDependency();
            builder.RegisterType<BTCTransactionService>().As<IBTCTransactionService>().InstancePerDependency();
            builder.RegisterType<ETHTransactionService>().As<IETHTransactionService>().InstancePerDependency();
            builder.RegisterType<SettingService>().As<ISettingService>().InstancePerDependency();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWorkAsync>().InstancePerLifetimeScope();
            builder.RegisterType<CPLContext>().As<IDataContextAsync>().InstancePerLifetimeScope();

            builder.RegisterType<Repository<CoinTransaction>>().As<IRepositoryAsync<CoinTransaction>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<SysUser>>().As<IRepositoryAsync<SysUser>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<BTCTransaction>>().As<IRepositoryAsync<BTCTransaction>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<ETHTransaction>>().As<IRepositoryAsync<ETHTransaction>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<Setting>>().As<IRepositoryAsync<Setting>>().InstancePerLifetimeScope();

            var container = builder.Build();
            return new Resolver(
                container,
                container.Resolve<IUnitOfWorkAsync>(),
                container.Resolve<ISysUserService>(),
                container.Resolve<IBTCTransactionService>(),
                container.Resolve<IETHTransactionService>(),
                container.Resolve<ISettingService>(),
                container.Resolve<ICoinTransactionService>()
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
            BTCDepositFileName = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "btc_deposit_log.txt");
            //ETHDepositFileName = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "eth_deposit_log.txt");
            BTCWithdrawFileName = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "btc_withdraw_log.txt");
            //ETHWithdrawFileName = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "eth_withdraw_log.txt");

            RunningIntervalInMilliseconds = int.Parse(Configuration["RunningIntervalInMilliseconds"]);
            NumberOfConfirmsForUnreversedBTCTransaction = int.Parse(Configuration["NumberOfConfirmsForUnreversedBTCTransaction"]);
            NumberOfDaysFailTransaction = int.Parse(Configuration["NumberOfDaysFailTransaction"]);
            ConnectionString = Configuration["ConnectionString"];
        }
    }
}
