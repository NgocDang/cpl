using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class LotteryResultViewModel
    {
        public int LastestLotteryId { get; set; }
        public bool? LastestLotteryStatus { get; set; }
        public string LastestLotteryTitle { get; set; }
        public string LastestLotteryResult { get; set; }
    }
}
