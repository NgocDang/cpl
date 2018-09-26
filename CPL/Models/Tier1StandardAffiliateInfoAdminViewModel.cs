using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class Tier1StandardAffiliateInfoAdminViewModel
    {
        public int TotalAffiliateSale { get; set; }
        public int DirectAffiliateSale { get; set; }
        public int TotalIntroducedUsers { get; set; }
        public int DirectIntroducedUsers { get; set; }

        public string TotalAffiliateSaleChangesInJson { get; set; }
        public string DirectAffiliateSaleChangesInJson { get; set; }
        public string TotalIntroducedUsersChangesInJson { get; set; }
        public string DirectIntroducedUsersChangesInJson { get; set; }
    }
}
