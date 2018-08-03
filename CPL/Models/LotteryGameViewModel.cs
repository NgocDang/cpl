using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class LotteryGameViewModel
    {
        public List<LotteryViewModel> Lotteries { get; set; }
        public int? SysUserId { get; set; }
        public string PrecentOfPerchasedTickets { get; set; }
    }
}
