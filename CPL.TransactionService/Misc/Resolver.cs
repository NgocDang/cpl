using Autofac;
using CPL.Core.Interfaces;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.TransactionService.Misc
{
    public class Resolver
    {
        public IContainer Container { get; }
        public IUnitOfWorkAsync UnitOfWork { get; }
        public ISysUserService SysUserService { get; }
        public ISettingService SettingService { get; }
        public IBTCTransactionService BTCTransactionService { get; }
        //public IETHTransactionService ETHTransactionService { get; }
        public ICoinTransactionService CoinTransactionService { get; }

        public Resolver(IContainer container, IUnitOfWorkAsync unitOfWork, ISysUserService sysUserService, 
            IBTCTransactionService btcTransactionService, IETHTransactionService ethTransactionService,
            ISettingService settingService,
            ICoinTransactionService coinTransactionService)
        {
            Container = container;
            UnitOfWork = unitOfWork;
            SysUserService = sysUserService;
            BTCTransactionService = btcTransactionService;
            //ETHTransactionService = ethTransactionService;
            CoinTransactionService = coinTransactionService;
            SettingService = settingService;
        }
    }
}
