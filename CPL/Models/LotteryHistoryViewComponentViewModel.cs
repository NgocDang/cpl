using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class LotteryHistoryViewComponentViewModel
    {
        public int? SysUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LotteryId { get; set; }
    }
}
