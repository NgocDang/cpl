using Autofac;
using CPL.Core.Interfaces;
using CPL.Infrastructure.Interfaces;

namespace CPL.SmartContractService.Misc
{
    public class Resolver
    {
        public static IContainer Container { get; set; }
        public static IUnitOfWorkAsync UnitOfWork { get; set; }
        public static ILotteryHistoryService LotteryHistoryService { get; set; }
        public static ILotteryService LotteryService { get; set; }
        public static ISysUserService SysUserService { get; set; }
    }
}
