using CPL.Common.Enums;
using CPL.Common.Misc;
using CPL.Core.Services;
using CPL.Domain;
using CPL.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
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
            var paymentFileName = (string)dataMap["PaymentFileName"];
            DoProcessPayment(paymentFileName, ref resolver);
            return Task.FromResult(0);
        }

        public void DoProcessPayment(string paymentFileName, ref Resolver resolver)
        {
            var payments = resolver.PaymentService.Query()
                .Include(x=>x.SysUser)
                .ThenInclude(y => y.Affiliate)
                .Include(x => x.SysUser)
                .ThenInclude(y => y.Agency)
                .Where(x => !x.UpdatedDate.HasValue && (x.SysUser.Affiliate.IsAutoPaymentEnable || x.SysUser.Agency.IsAutoPaymentEnable))
                .ToList();

            foreach(var payment in payments)
            {
                var commission = payment.Tier1DirectRate * payment.Tier1DirectSale
                    + payment.Tier2SaleToTier1Rate * payment.Tier2SaleToTier1Sale
                    + payment.Tier3SaleToTier1Rate * payment.Tier3SaleToTier1Sale;

                if (commission >0)
                    payment.SysUser.TokenAmount += commission;
                resolver.SysUserService.Update(payment.SysUser);

                payment.UpdatedDate = DateTime.Now;
                resolver.PaymentService.Update(payment);
            }

            resolver.UnitOfWork.SaveChanges();
            Utils.FileAppendThreadSafe(paymentFileName, string.Format("All payments are paid at: {0}{1}{2}", DateTime.Now, Environment.NewLine, Environment.NewLine));
        }
    }
}
