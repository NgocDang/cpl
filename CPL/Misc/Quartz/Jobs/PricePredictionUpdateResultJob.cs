using CPL.Core.Services;
using CPL.Infrastructure.Repositories;
using CPL.Misc.Quartz.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc.Quartz.Jobs
{
    public class PricePredictionUpdateResultJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            BTCPricePredictionUpdateResult(context);
            return Task.FromResult(0);
        }

        public void BTCPricePredictionUpdateResult(IJobExecutionContext context)
        {
            var lotteryService = ((PricePredictionService)context.JobDetail.JobDataMap["PricePredictionService"]);
            var sysUserService = ((PricePredictionHistoryService)context.JobDetail.JobDataMap["PricePredictionHistoryService"]);
            var unitOfWork = ((UnitOfWork)context.JobDetail.JobDataMap["UnitOfWork"]);


        }
    }
}
