using CPL.Misc.Quartz.Interfaces;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc.Quartz
{
    public interface IQuartzSchedulerService
    {
        IScheduler GetScheduler<Scheduler, JobFactory>()
            where Scheduler : IScheduler
            where JobFactory : IJobFactory;
    }

    public class QuartzSchedulerService : IQuartzSchedulerService
    {
        private readonly IServiceProvider _serviceProvider;

        public QuartzSchedulerService(
            IServiceProvider serviceProvider
            )
        {
            _serviceProvider = serviceProvider;
        }

        public IScheduler GetScheduler<Scheduler, JobFactory>()
            where Scheduler : IScheduler
            where JobFactory : IJobFactory

        {
            return _serviceProvider.GetScheduler<Scheduler, JobFactory>();
        }
    }
}
