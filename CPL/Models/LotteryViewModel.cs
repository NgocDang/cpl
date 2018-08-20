using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class LotteryViewModel
    {
        public int Id { get; set; }
        public int Phase { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Volume { get; set; }
        public int Status { get; set; }
        public string DesktopSlideImage { get; set; }
        public string MobileSlideImage { get; set; }
        public string DesktopListingImage { get; set; }
        public string MobileListingImage { get; set; }
        public string PrizeImage { get; set; }
        public string Title { get; set; }
        public int UnitPrice { get; set; }

        public string CreatedDateInString { get; set; }

        public IFormFile DesktopSlideImageFile { get; set; }
        public IFormFile MobileSlideImageFile { get; set; }
        public IFormFile PrizeImageFile { get; set; }
        public IFormFile DesktopListingImageFile { get; set; }
        public IFormFile MobileListingImageFile { get; set; }
        public bool IsPublished { get; set; }

        public List<LotteryHistoryViewModel> LotteryHistories { get; set; }
        public List<LotteryPrizeViewModel> LotteryPrizes { get; set; }
    }

    public class Prizes
    {
        public decimal NumberOfTicketWin { get; set; }
        public decimal PrizeProbability { get; set; }
    }
}
