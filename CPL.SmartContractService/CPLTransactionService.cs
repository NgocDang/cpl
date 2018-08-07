using Autofac;
using CPL.Common.Enums;
using CPL.Core.Interfaces;
using CPL.Core.Services;
using CPL.Domain;
using CPL.Infrastructure;
using CPL.Infrastructure.Interfaces;
using CPL.Infrastructure.Repositories;
using CPL.SmartContractService.Misc;
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

namespace CPL.SmartContractService
{
    public class CPLTransactionService : MicroService, IMicroService
    {
        public static IConfiguration Configuration { get; set; }
        public static bool IsSmartContractServiceRunning = false;
        public static List<Task> Tasks = new List<Task>();

        public string FileName { get; set; }
        public string ConnectionString { get; set; }
        public int RunningIntervalInMilliseconds { get; set; }
        public string ServiceEnvironment { get; set; }

        public static AuthenticationService.AuthenticationClient _authentication = new AuthenticationService.AuthenticationClient();
        public static ETokenService.ETokenClient _eToken = new ETokenService.ETokenClient();
        public static ETransactionService.ETransactionClient _eTransaction = new ETransactionService.ETransactionClient();

        public readonly int NumberOfDigits = 6;

        public void Start()
        {
            // ConfigurationBuilder
            ConfigurationBuilder();

            // Initialize
            InitializeSetting();

            // write log
            Utils.FileAppendThreadSafe(FileName, String.Format("{0} started at {1}{2}", WSConstant.ServiceName, DateTime.Now, Environment.NewLine));

            //Init dependency transaction & dbcontext
            InitializeRepositories();

            //Init setting
            IsSmartContractServiceRunning = true;
            Tasks.Clear();
            Tasks.Add(Task.Run(() => CheckTransaction()));
        }

        public void Stop()
        {
            IsSmartContractServiceRunning = false;
            Utils.FileAppendThreadSafe(FileName, string.Format("Stop main thread at : {0}{1}{2}", DateTime.Now, Environment.NewLine, Environment.NewLine));
            Task.WaitAll(Tasks.ToArray());
        }

        private void CheckTransaction()
        {
            try
            {
                Utils.FileAppendThreadSafe(FileName, string.Format("Check Tx thread STARTED on {0}{1}", DateTime.Now, Environment.NewLine));

                var authentication = new AuthenticationService.AuthenticationClient().AuthenticateAsync(WSConstant.Email, WSConstant.ProjectName);
                authentication.Wait();

                if (authentication.Result.Status.Code == 0)
                {
                    Utils.FileAppendThreadSafe(FileName, string.Format("Successful authentication with token {0} at {1}{2}", authentication.Result.Token, DateTime.Now, Environment.NewLine));
                    var eToken = new ETokenService.ETokenClient().SetAsync(authentication.Result.Token, new ETokenService.ETokenSetting { Abi = CPLConstant.Abi, ContractAddress = CPLConstant.SmartContractAddress, Environment = (ServiceEnvironment == ETokenService.Environment.MAINNET.ToString() ? ETokenService.Environment.MAINNET : ETokenService.Environment.TESTNET), Platform = ETokenService.Platform.ETH });
                    eToken.Wait();
                    var eTransaction = new ETransactionService.ETransactionClient().SetAsync(authentication.Result.Token, new ETransactionService.ETransactionSetting { ApiKey = CPLConstant.ETransactionAPIKey, Environment = (ServiceEnvironment == ETransactionService.Environment.MAINNET.ToString() ? ETransactionService.Environment.MAINNET : ETransactionService.Environment.TESTNET), Platform = ETransactionService.Platform.ETH });
                    eTransaction.Wait();

                    do
                    {
                        var transactionList = Resolver.LotteryHistoryService.Queryable()
                            .Where(x => string.IsNullOrEmpty(x.TicketNumber))
                            .Select(x => x.TxHashId).Distinct().ToList(); // In case user buys multiple lottery tickets

                        foreach(var tx in transactionList)
                        {
                            var txStatus = _eTransaction.GetTransactionStatusAsync(authentication.Result.Token, tx);
                            txStatus.Wait();
                            if(txStatus.Result.Status.Code == 0)
                            {
                                if(txStatus.Result.Receipt == true)
                                {
                                    var duplicateTicketIndexList = new List<int>();
                                    var lotteryPhase = Resolver.LotteryHistoryService.Queryable().Include(x => x.Lottery).FirstOrDefault(x => x.TxHashId == tx).Lottery.Phase;
                                    var lotteryId = Resolver.LotteryHistoryService.Queryable().Include(x => x.Lottery).FirstOrDefault(x => x.TxHashId == tx).Lottery.Id;
                                    var userAddress = Resolver.LotteryHistoryService.Queryable().Include(x => x.SysUser).FirstOrDefault(x => x.TxHashId == tx).SysUser.ETHHDWalletAddress;

                                    var lotteryHistories = Resolver.LotteryHistoryService.Queryable().Where(x => x.TxHashId == tx).ToList();
                                    foreach(var lotteryHistory in lotteryHistories)
                                    {
                                        var getTicketParamJson = string.Format(CPLConstant.GetTicketParamInJson, lotteryPhase, userAddress, lotteryHistory.TicketIndex);
                                        var ticketNumber = _eToken.CallFunctionAsync(authentication.Result.Token, "getTicket", getTicketParamJson);
                                        ticketNumber.Wait();

                                        if (ticketNumber.Result.Status.Code == 0)
                                        {
                                            var tickerNumber = CorrectTicketNumber(ticketNumber.Result.Result);
                                            if(!Resolver.LotteryHistoryService.Queryable().Any(x => x.TicketNumber == tickerNumber && x.LotteryId == lotteryHistory.LotteryId))
                                                lotteryHistory.TicketNumber = tickerNumber;
                                            else
                                                duplicateTicketIndexList.Add(lotteryHistory.TicketIndex);
                                        }
                                        Resolver.LotteryHistoryService.Update(lotteryHistory);
                                    }

                                    if (duplicateTicketIndexList.Count > 0)
                                    {
                                        var randomParamJson = string.Format(CPLConstant.RandomParamInJson, lotteryPhase, userAddress, string.Join(",", duplicateTicketIndexList));
                                        var ticketRandomResult = _eToken.CallTransactionAsync(authentication.Result.Token,
                                            CPLConstant.OwnerAddress,
                                            CPLConstant.OwnerPassword,
                                            "random",
                                            CPLConstant.GasPriceMultiplicator,
                                            CPLConstant.DurationInSecond,
                                            randomParamJson);

                                        ticketRandomResult.Wait();
                                        if (ticketRandomResult.Result.Status.Code == 0)
                                        {
                                            //Performance issue
                                            for (int i = 0; i < duplicateTicketIndexList.Count; i++)
                                            {
                                                var lotteryHistory = Resolver.LotteryHistoryService.Queryable().FirstOrDefault(x => x.TicketIndex == duplicateTicketIndexList[i] && x.LotteryId == lotteryId);
                                                lotteryHistory.TxHashId = ticketRandomResult.Result.TxId;
                                                Resolver.LotteryHistoryService.Update(lotteryHistory);
                                            }
                                        }
                                    }
                                }
                                else if(txStatus.Result.Receipt == false)
                                {
                                    int lotteryPhase = Resolver.LotteryHistoryService.Queryable().Include(x => x.Lottery).FirstOrDefault(x => x.TxHashId == tx).Lottery.Phase;
                                    string userAddress = Resolver.LotteryHistoryService.Queryable().Include(x => x.SysUser).FirstOrDefault(x => x.TxHashId == tx).SysUser.ETHHDWalletAddress;

                                    var ticketIndexList = Resolver.LotteryHistoryService.Queryable().Where(x => x.TxHashId == tx).Select(x => x.TicketIndex).ToList();
                                    var randomParamJson = string.Format(CPLConstant.RandomParamInJson, lotteryPhase, userAddress, string.Join(",", ticketIndexList));

                                    var ticketRandomResult = _eToken.CallTransactionAsync(authentication.Result.Token, 
                                        CPLConstant.OwnerAddress, 
                                        CPLConstant.OwnerPassword, 
                                        "random", 
                                        CPLConstant.GasPriceMultiplicator, 
                                        CPLConstant.DurationInSecond, 
                                        randomParamJson);

                                    ticketRandomResult.Wait();
                                    var lotteryId = Resolver.LotteryHistoryService.Queryable().FirstOrDefault(x => x.TxHashId == tx).LotteryId;

                                    if (ticketRandomResult.Result.Status.Code == 0)
                                    {
                                        for (int i = 0; i < ticketIndexList.Count; i++)
                                        {
                                            var lotteryHistoryRecord = Resolver.LotteryHistoryService.Queryable().FirstOrDefault(x => x.TicketIndex == ticketIndexList[i] && x.LotteryId == lotteryId);
                                            lotteryHistoryRecord.TxHashId = ticketRandomResult.Result.TxId;
                                            Resolver.LotteryHistoryService.Update(lotteryHistoryRecord);
                                        }
                                    }
                                }
                            }
                        }
                        Resolver.UnitOfWork.SaveChanges();
                        Thread.Sleep(RunningIntervalInMilliseconds);
                    }
                    while (IsSmartContractServiceRunning);
                }
                else
                    Utils.FileAppendThreadSafe(FileName, string.Format("Authentication failed at {0}. Reason {1}{2}", DateTime.Now, authentication.Result.Status.Text, Environment.NewLine));
            }
            catch(Exception ex)
            {
                if (ex.InnerException.Message != null)
                    Utils.FileAppendThreadSafe(FileName, string.Format("Exception {0} at {1}{2}", ex.InnerException.Message, DateTime.Now, Environment.NewLine));
                else
                    Utils.FileAppendThreadSafe(FileName, string.Format("Exception {0} at {1}{2}", ex.Message, DateTime.Now, Environment.NewLine));
            }
            
            Utils.FileAppendThreadSafe(FileName, string.Format("Check Tx thread STOPPED at {0}{1}", DateTime.Now, Environment.NewLine));
        }

        /// <summary>
        /// Corrects the ticket number.
        /// </summary>
        /// <param name="tickerNumber">The ticker number.</param>
        /// <returns></returns>
        private string CorrectTicketNumber(string tickerNumber)
        {
            if (tickerNumber.Length < NumberOfDigits)
            {
                do
                {
                    tickerNumber = tickerNumber.Insert(0, "0");
                } while (tickerNumber.Length < NumberOfDigits);
            }
            return tickerNumber;
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

            builder.RegisterType<LotteryHistoryService>().As<ILotteryHistoryService>().InstancePerLifetimeScope();
            builder.RegisterType<LotteryService>().As<ILotteryService>().InstancePerLifetimeScope();
            builder.RegisterType<SysUserService>().As<ISysUserService>().InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWorkAsync>().InstancePerLifetimeScope();
            builder.RegisterType<CPLContext>().As<IDataContextAsync>().InstancePerLifetimeScope();

            builder.RegisterType<Repository<LotteryHistory>>().As<IRepositoryAsync<LotteryHistory>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<Lottery>>().As<IRepositoryAsync<Lottery>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<SysUser>>().As<IRepositoryAsync<SysUser>>().InstancePerLifetimeScope();

            Resolver.Container = builder.Build();
            Resolver.UnitOfWork = Resolver.Container.Resolve<IUnitOfWorkAsync>();
            Resolver.LotteryHistoryService = Resolver.Container.Resolve<ILotteryHistoryService>();
            Resolver.LotteryService = Resolver.Container.Resolve<ILotteryService>();
            Resolver.SysUserService = Resolver.Container.Resolve<ISysUserService>();
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
