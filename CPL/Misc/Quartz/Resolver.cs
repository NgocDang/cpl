using CPL.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc.Quartz
{
    public static class Resolver
    {
        public static ILotteryService LotteryService { get; set; }
        public static ILotteryPrizeService LotteryPrizeService { get; set; }
        public static ILotteryHistoryService LotteryHistoryService { get; set; }
    }
}
