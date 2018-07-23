using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc
{
    public class LotteryRawingJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            ILotteryRawingService lotteryRawingService = new LotteryRawingService();

            return Task.Run(() => lotteryRawingService.Rawing());
        }
    }
}
