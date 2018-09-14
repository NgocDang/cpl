using CPL.Common.Enums;
using CPL.Common.Misc;
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
            var dataMap = context.JobDetail.JobDataMap;
            var resolver = (Resolver)dataMap["Resolver"];
            var paymentFileName = (string)dataMap["PaymentFileName"];
            DoCreatePayment(paymentFileName, ref resolver);
            return Task.FromResult(0);
        }

        public void DoCreatePayment(string paymentFileName, ref Resolver resolver)
        {
            resolver.CPLContext.ExecuteSqlCommand("exec usp_CreatePayment");
            Utils.FileAppendThreadSafe(paymentFileName, string.Format("All payment are generated at: {0}{1}{2}", DateTime.Now, Environment.NewLine, Environment.NewLine));
        }
    }
}
