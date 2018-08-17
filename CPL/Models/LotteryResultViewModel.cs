using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class LotteryResultViewModel
    {
        public decimal ETHAmount { get; set; }
        public decimal BTCAmount { get; set; }
        public decimal TokenAmount { get; set; }
        public decimal BTCToTokenRate { get; set; }
        public decimal ETHToTokenRate { get; set; }

        public bool? Status { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Result { get; set; }
    }
}
