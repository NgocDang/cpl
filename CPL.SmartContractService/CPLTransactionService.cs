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
            IsSmartContractServiceRunning = true;
            Tasks.Clear();
            Tasks.Add(Task.Run(() => CheckTransactions()));
        }

        public void Stop()
        {
            IsSmartContractServiceRunning = false;
            Utils.FileAppendThreadSafe(FileName, string.Format("Stop main thread at : {0}{1}{2}", DateTime.Now, Environment.NewLine, Environment.NewLine));
            Task.WaitAll(Tasks.ToArray());
        }

        private void CheckTransactions()
        {
            try
            {
                var authentication = new AuthenticationService.AuthenticationClient().AuthenticateAsync(WSConstant.Email, WSConstant.ProjectName);
                authentication.Wait();

                if (authentication.Result.Status.Code == 0)
                {
                    Utils.FileAppendThreadSafe(FileName, string.Format("Check Tx thread STARTED on {0}{1}", DateTime.Now, Environment.NewLine));

                    var eToken = new ETokenService.ETokenClient().SetAsync(authentication.Result.Token, new ETokenService.ETokenSetting { Abi = CPLConstant.Abi, ContractAddress = CPLConstant.SmartContractAddress, Environment = (ServiceEnvironment == ETokenService.Environment.MAINNET.ToString() ? ETokenService.Environment.MAINNET : ETokenService.Environment.TESTNET), Platform = ETokenService.Platform.ETH });
                    eToken.Wait();
                    var eTransaction = new ETransactionService.ETransactionClient().SetAsync(authentication.Result.Token, new ETransactionService.ETransactionSetting { ApiKey = CPLConstant.ETransactionAPIKey, Environment = (ServiceEnvironment == ETransactionService.Environment.MAINNET.ToString() ? ETransactionService.Environment.MAINNET : ETransactionService.Environment.TESTNET), Platform = ETransactionService.Platform.ETH });
                    eTransaction.Wait();

                    do
                    {
                        var transactionList = Resolver.LotteryHistoryService.Queryable().Where(x => string.IsNullOrEmpty(x.TicketNumber)).Select(x => x.TxHashId).Distinct().ToList();
                        foreach(var tx in transactionList)
                        {
                            var txStatus = _eTransaction.GetTransactionStatusAsync(authentication.Result.Token, tx);
                            txStatus.Wait();
                            if(txStatus.Result.Status.Code == 0)
                            {
                                if(txStatus.Result.Receipt == true)
                                {
                                    var duplicateTicketIndexList = new List<int>();
                                    var lotteryPhase = Resolver.LotteryHistoryService.Queryable().Include(x => x.Lottery).Where(x => x.TxHashId == tx).FirstOrDefault().Lottery.Phase;
                                    var lotteryId = Resolver.LotteryHistoryService.Queryable().Include(x => x.Lottery).Where(x => x.TxHashId == tx).FirstOrDefault().Lottery.Id;
                                    var userAddress = Resolver.LotteryHistoryService.Queryable().Include(x => x.SysUser).Where(x => x.TxHashId == tx).FirstOrDefault().SysUser.ETHHDWalletAddress;

                                    var lotteryRecords = Resolver.LotteryHistoryService.Queryable().Where(x => x.TxHashId == tx).ToList();
                                    foreach(var lotto in lotteryRecords)
                                    {
                                        var getTicketParamJson = CPLConstant.getTicketParamInJson.Replace("lotteryphase", lotteryPhase.ToString()).Replace("useraddress", userAddress).Replace("ticketindex", lotto.TicketIndex.ToString());
                                        var ticketNumber = _eToken.CallFunctionAsync(authentication.Result.Token, "getTicket", getTicketParamJson);
                                        ticketNumber.Wait();

                                        if (ticketNumber.Result.Status.Code == 0)
                                        {
                                            StringBuilder sb = new StringBuilder();
                                            var tmpTicketNo = ticketNumber.Result.Result;

                                            if(tmpTicketNo.Length < 6 )
                                            {
                                                do
                                                {
                                                    tmpTicketNo = tmpTicketNo.Insert(0, "0");
                                                } while (tmpTicketNo.Length < 6);
                                            }

                                            var duplicateRecords = Resolver.LotteryHistoryService.Queryable().Where(x => x.TicketNumber == tmpTicketNo && x.LotteryId == lotto.LotteryId).FirstOrDefault();
                                            if(duplicateRecords == null)
                                            {
                                                lotto.TicketNumber = tmpTicketNo;
                                            }
                                            else
                                            {
                                                duplicateTicketIndexList.Add(lotto.TicketIndex);
                                            }
                                        }
                                        Resolver.LotteryHistoryService.Update(lotto);
                                    }

                                    string ticketList = string.Join(",", duplicateTicketIndexList.ToArray());
                                    var randomParamJson = CPLConstant.randomParamInJson.Replace("lotteryphase", lotteryPhase.ToString()).Replace("useraddress", userAddress).Replace("ticketindexlist", ticketList);

                                    var ticketGenResult = _eToken.CallTransactionAsync(authentication.Result.Token, CPLConstant.OwnerAddress, CPLConstant.OwnerPassword, "random", CPLConstant.GasPriceMultiplicator, CPLConstant.DurationInSecond, randomParamJson);
                                    ticketGenResult.Wait();

                                    if(ticketGenResult.Result.Status.Code == 0)
                                    {
                                        for(int i = 0; i < duplicateTicketIndexList.Count; i++)
                                        {
                                            var lotteryRecord = Resolver.LotteryHistoryService.Queryable().Where(x => x.TicketIndex == duplicateTicketIndexList[i] && x.LotteryId == lotteryId).FirstOrDefault();
                                            lotteryRecord.TxHashId = ticketGenResult.Result.TxId;
                                            Resolver.LotteryHistoryService.Update(lotteryRecord);
                                        }
                                    }
                                }
                                else if(txStatus.Result.Receipt == false)
                                {
                                    var ticketIndexList = new List<int>();
                                    int lotteryPhase = Resolver.LotteryHistoryService.Queryable().Include(x => x.Lottery).Where(x => x.TxHashId == tx).FirstOrDefault().Lottery.Phase;
                                    string userAddress = Resolver.LotteryHistoryService.Queryable().Include(x => x.SysUser).Where(x => x.TxHashId == tx).FirstOrDefault().SysUser.ETHHDWalletAddress;

                                    var lotteryRecords = Resolver.LotteryHistoryService.Queryable().Where(x => x.TxHashId == tx).ToList();
                                    foreach (var lotto in lotteryRecords)
                                    {
                                        ticketIndexList.Add(lotto.TicketIndex);
                                    }
                                    string ticketList = string.Join(",", ticketIndexList.ToArray());
                                    var randomParamJson = CPLConstant.randomParamInJson.Replace("lotteryphase", lotteryPhase.ToString()).Replace("useraddress", userAddress).Replace("ticketindexlist", ticketList);

                                    var ticketGenResult = _eToken.CallTransactionAsync(authentication.Result.Token, CPLConstant.OwnerAddress, CPLConstant.OwnerPassword, "random", CPLConstant.GasPriceMultiplicator, CPLConstant.DurationInSecond, randomParamJson);
                                    ticketGenResult.Wait();

                                    var lotteryId = Resolver.LotteryHistoryService.Queryable().Include(x => x.Lottery).Where(x => x.TxHashId == tx).FirstOrDefault().Lottery.Id;

                                    if (ticketGenResult.Result.Status.Code == 0)
                                    {
                                        for (int i = 0; i < ticketIndexList.Count; i++)
                                        {
                                            var lotteryRecord = Resolver.LotteryHistoryService.Queryable().Where(x => x.TicketIndex == ticketIndexList[i] && x.LotteryId == lotteryId).FirstOrDefault();
                                            lotteryRecord.TxHashId = ticketGenResult.Result.TxId;
                                            Resolver.LotteryHistoryService.Update(lotteryRecord);
                                        }
                                    }
                                }
                            }
                        }
                        Resolver.UnitOfWork.SaveChanges();
                        Thread.Sleep(60000);
                    }
                    while (IsSmartContractServiceRunning);
                }
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
            ServiceEnvironment = Configuration["Environment"];
        }
    }
}
