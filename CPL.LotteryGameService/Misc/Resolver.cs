using Autofac;
using CPL.Core.Interfaces;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.LotteryGameService.Misc
{
    public class Resolver
    {
        public static IContainer Container { get; set; }
        public static IUnitOfWorkAsync UnitOfWork { get; set; }

        public static ILotteryService LotteryService { get; set; }
        public static ILotteryHistoryService LotteryHistoryService { get; set; }
        public static ILotteryPrizeService LotteryPrizeService { get; set; }
        public static ISettingService SettingService { get; set; }
    }
}
