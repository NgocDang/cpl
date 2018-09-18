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
        public static IContainer Container { get; set; }
        public static IUnitOfWorkAsync UnitOfWork { get; set; }
        public static ISysUserService SysUserService { get; set; }
        public static IBTCTransactionService BTCTransactionService { get; set; }
        //public static IETHTransactionService ETHTransactionService { get; set; }
        public static ISettingService SettingService { get; set; }
    }
}
