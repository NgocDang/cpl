using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class LotteryViewModel
    {
        public int Id { get; set; }
        public int Volume { get; set; }
        public string DesktopTopImage { get; set; }
        public string MobileTopImage { get; set; }
        public string PrizeImage { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public List<LotteryHistoryViewModel> LotteryHistories { get; set; }
        public List<LotteryDetailViewModel> LotteryDetails { get; set; }

        public int? SysUserId { get; set; }
        public string PrecentOfPerchasedTickets { get; set; }
        public int UnitPrice { get; set; }
    }
}
