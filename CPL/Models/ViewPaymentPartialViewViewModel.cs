using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class ConfirmPaymentPartialViewViewModel
    {
        public int SysUserId { get; set; }
        public string CommissionAmount { get; set; }
        public string Period { get; set; }
    }
}
