using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.PaymentService.Misc
{
    public class PaymentServiceConstant
    {
        public const string ServiceName = "CPL Payment Service";
        public const string ServiceDescription = "To generate payments at the beginning of every month and execute them on 10th monthly.";

        public const string PaymentMonthlyStartTimeInDay = "PaymentMonthlyStartTimeInDay";
        public const string PaymentMonthlyStartTimeInHour = "PaymentMonthlyStartTimeInHour";
        public const string PaymentMonthlyStartTimeInMinute = "PaymentMonthlyStartTimeInMinute";
    }
}
