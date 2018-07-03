using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class WithdrawViewModel
    {
        public decimal Amount { get; set; }
        public string Address { get; set; }
        public string Currency { get; set; }
    }
}
