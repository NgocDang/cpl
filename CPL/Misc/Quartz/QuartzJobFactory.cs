using CPL.Core.Interfaces;
using CPL.Infrastructure.Interfaces;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc.Quartz
{
    public class QuartzJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public QuartzJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobDetail = bundle.JobDetail;

            jobDetail.JobDataMap["SysUserService"] = _serviceProvider.GetService(typeof(ISysUserService));
            jobDetail.JobDataMap["LotteryService"] = _serviceProvider.GetService(typeof(ILotteryService));
            jobDetail.JobDataMap["LotteryPrizeService"] = _serviceProvider.GetService(typeof(ILotteryPrizeService));
            jobDetail.JobDataMap["LotteryHistoryService"] = _serviceProvider.GetService(typeof(ILotteryHistoryService));
            jobDetail.JobDataMap["UnitOfWork"] = _serviceProvider.GetService(typeof(IUnitOfWorkAsync));

            return (IJob)_serviceProvider.GetService(jobDetail.JobType);
        }

        public void ReturnJob(IJob job) { }
    }
}
