using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class HomeViewModel
    {
        public List<HomeSlideViewModel> Slides { get; set; }
        public List<HomeLotteryViewModel> Lotteries { get; set; }
        public NewsViewModel News { get; set; }
    }

    public class HomeSlideViewModel
    {
        public int Id { get; set; }
        public string DesktopSlideImage { get; set; }
        public string MobileSlideImage { get; set; }
    }

    public class HomeLotteryViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Phase { get; set; }
        public int NumberOfTicketLeft { get; set; }
        public string DesktopListingImage { get; set; }
        public string MobileListingImage { get; set; }
        public List<LotteryDetailViewModel> LotteryDetails {get;set;}
    }
}
