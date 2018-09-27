using Autofac;
using CPL.Core.Interfaces;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.NotifyService.Misc
{
    public class Resolver
    {
        public IContainer Container { get; set; }
        public IUnitOfWorkAsync UnitOfWork { get; set; }
        public ISysUserService SysUserService { get; set; }
        public IBTCTransactionService BTCTransactionService { get; set; }
        //public static IETHTransactionService ETHTransactionService { get; set; }
        public ISettingService SettingService { get; set; }

        public Resolver(IContainer container, IUnitOfWorkAsync unitOfWork, ISysUserService sysUserService,
            IBTCTransactionService btcTransactionService, ISettingService settingService)
            //IETHTransactionService ethTransactionService,)
        {
            Container = container;
            UnitOfWork = unitOfWork;
            SysUserService = sysUserService;
            BTCTransactionService = btcTransactionService;
            //ETHTransactionService = ethTransactionService;
            SettingService = settingService;
        }
    }
}
