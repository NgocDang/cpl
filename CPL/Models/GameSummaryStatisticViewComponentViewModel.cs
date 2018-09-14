using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class GameSummaryStatisticViewComponentViewModel
    {
        public int TotalRevenue { get; set; }
        public int TotalSale { get; set; }
        public int PageView { get; set; }
        public int TotalPlayers { get; set; }
        public int TodayPlayers { get; set; }
    }
}
