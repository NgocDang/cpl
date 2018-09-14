using CPL.Common.Enums;
using CPL.Core.Services;
using CPL.Domain;
using CPL.Infrastructure;
using CPL.Infrastructure.Interfaces;
using CPL.Infrastructure.Repositories;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPL.PaymentService.Misc.Quartz.Jobs
{
    internal class PaymentCreateJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            Resolver resolver = (Resolver)dataMap["Resolver"];
            DoCreatePayment(ref resolver);
            return Task.FromResult(0);
        }

        public void DoCreatePayment(ref Resolver resolver)
        {
            var payment = resolver.PaymentService.SelectQuery("usp_CreatePayment");
        }
    }
}
