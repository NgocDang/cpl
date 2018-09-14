using CPL.Misc.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class GameManagementIndexViewModel
    {
        // Summary
        
    }

    public class SummaryChangesViewModel
    {
        public List<SummaryChange> TotalRevenueChanges { get; set; }
        public List<SummaryChange> TotalSaleChanges { get; set; }
        public List<PageViewsViewModel> PageViewChanges { get; set; }
        public List<SummaryChange> TotalPlayersChanges { get; set; }
    }

    public class SummaryChange
    {
        public DateTime Date { get; set; }
        public int Value { get; set; }
    }

    public class PlayersChange
    {
        public DateTime Date { get; set; }
        public IEnumerable<int> SysUserIdList { get; set; }
    }
}
