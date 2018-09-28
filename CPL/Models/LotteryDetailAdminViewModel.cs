using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class LotteryDetailAdminViewModel
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

        public LangViewModel Lang { get; set; }
        public IFormFile DesktopTopImageFile { get; set; }
        public IFormFile MobileTopImageFile { get; set; }
        public IFormFile PrizeImageFile { get; set; }
        public IFormFile DesktopListingImageFile { get; set; }
        public IFormFile MobileListingImageFile { get; set; }
    }
}
