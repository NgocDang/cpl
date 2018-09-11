using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class LotteryDetailViewModel
    {
        public int Id { get; set; }
        public int LotteryId { get; set; }
        public int LangId { get; set; }
        public string DesktopListingImage { get; set; }
        public string MobileListingImage { get; set; }
        public string DesktopTopImage { get; set; }
        public string MobileTopImage { get; set; }
        public string PrizeImage { get; set; }
        public string Description { get; set; }
    }
}
