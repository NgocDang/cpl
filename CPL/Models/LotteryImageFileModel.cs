using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class LotteryImageFileModel
    {
        public int LangId { get; set; }
        public IFormFile DesktopTopImageFile { get; set; }
        public IFormFile MobileTopImageFile { get; set; }
        public IFormFile PrizeImageFile { get; set; }
        public IFormFile DesktopListingImageFile { get; set; }
        public IFormFile MobileListingImageFile { get; set; }
    }
}
