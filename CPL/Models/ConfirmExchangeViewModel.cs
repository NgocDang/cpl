using CPL.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class ConfirmExchangeViewModel
    {
        public string FromCurrency { get; set; }
        public decimal FromAmount { get; set; }
        public string ToCurrency { get; set; }
        public decimal ToAmount { get; set; }
    }
}
