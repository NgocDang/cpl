using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class RateViewModel
    {
        public decimal BTCToTokenRate { get; set; }
        public decimal ETHToTokenRate { get; set; }
        public int TokenAmount { get; set; }

        public int SysUserId { get; set; }
    }
}
