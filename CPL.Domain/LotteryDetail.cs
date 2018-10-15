using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class LotteryDetail : Entity
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
        public string ShortDescription { get; set; }

        public virtual Lottery Lottery { get; set; }
        public virtual Lang Lang { get; set; }

    }
}
