using CPL.Common.Enums;
using CPL.Core.Services;
using CPL.Domain;
using CPL.Infrastructure.Repositories;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPL.PaymentService.Misc.Quartz.Jobs
{
    internal class PaymentProcessJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            Resolver resolver = (Resolver)dataMap["Resolver"];
            DoProcessPayment(ref resolver);
            return Task.FromResult(0);
        }

        public void DoProcessPayment(ref Resolver resolver)
        {

        }
    }
}
