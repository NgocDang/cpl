using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class ExchangeViewModel
    {
        public decimal ETHAmount { get; set; }
        public decimal BTCAmount { get; set; }
        public decimal TokenAmount { get; set; }
        public decimal BTCToTokenrate { get; set; }
        public decimal ETHToBTCRate { get; set; }
    }
}
