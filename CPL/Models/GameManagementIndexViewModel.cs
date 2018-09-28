using CPL.Misc.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class GameManagementIndexViewModel
    {
        public string Tab { get; set; }
        public List<LotteryCategoryAdminViewModel> LotteryCategories { get; set; }
    }
}
