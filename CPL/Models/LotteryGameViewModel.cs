using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class LotteryGameViewModel
    {
        public LotteryViewModel Lottery { get; set; }
        public int? SysUserId { get; set; }
        public string PrecentOfPerchasedTickets { get; set; }
        public int LangId { get; set; }
    }
}
