using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class TopAgencyAffiliateInfoViewModel
    {
        public int TotalSale { get; set; }
        public int DirectSale { get; set; }
        public int TotalIntroducedUsers { get; set; }
        public int DirectIntroducedUsers { get; set; }

        public string TotalSaleChangesInJson { get; set; }
        public string DirectSaleChangesInJson { get; set; }
        public string TotalIntroducedUsersChangesInJson { get; set; }
        public string DirectIntroducedUsersChangesInJson { get; set; }
    }
}
