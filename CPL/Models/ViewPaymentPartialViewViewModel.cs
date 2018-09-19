using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class ViewPaymentPartialViewViewModel
    {
        public int SysUserId { get; set; }
        public decimal CommissionAmount { get; set; }
        public string Period { get; set; }
    }
}
