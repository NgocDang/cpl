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

        public int LastestLotteryId { get; set; }
        public bool? LastestLotteryStatus { get; set; }
        public string LastestLotteryTitle { get; set; }
        public string LastestLotteryResult { get; set; }
    }
}
