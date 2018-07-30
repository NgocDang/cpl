using CPL.Core.Interfaces;
using CPL.Infrastructure.Interfaces;
using CPL.Misc.Quartz.Interfaces;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc.Quartz.Factories
{
    public class PricePredictionUpdateResultFactory : IPricePredictionUpdateResultFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PricePredictionUpdateResultFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobDetail = bundle.JobDetail;

            jobDetail.JobDataMap["PricePredictionService"] = _serviceProvider.GetService(typeof(IPricePredictionService));
            jobDetail.JobDataMap["PricePredictionHistoryService"] = _serviceProvider.GetService(typeof(IPricePredictionHistoryService));
            jobDetail.JobDataMap["UnitOfWork"] = _serviceProvider.GetService(typeof(IUnitOfWorkAsync));

            return (IJob)_serviceProvider.GetService(jobDetail.JobType);
        }

        public void ReturnJob(IJob job) { }
    }
}
