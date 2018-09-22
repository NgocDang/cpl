using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class TopAgencyAffiliateIntroducedUsersViewModel
    {
        public int Id { get; set; }
        public string KindOfTier { get; set; }
        public decimal UsedCPL { get; set; }
        public decimal LostCPL { get; set; }
        public decimal AffiliateSale { get; set; }
        public int TotalIntroducedUsers { get; set; }
        public string AffiliateCreatedDateInString { get; set; }
    }
}
