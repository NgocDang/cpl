using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class TopAgencyAffiliateIntroducedUsersAdminViewModel
    {
        public int Id { get; set; }
        public string KindOfTier { get; set; }
        public decimal UsedCPL { get; set; }
        public decimal LostCPL { get; set; }
        public decimal AffiliateSale { get; set; }
        public int TotalDirectIntroducedUsers { get; set; }
        public string AffiliateCreatedDateInString { get; set; }

        public int AffiliateId { get; set; }
        public int? Tier1DirectRate { get; set; }
        public int? Tier2SaleToTier1Rate { get; set; }
        public int? Tier3SaleToTier1Rate { get; set; }

        public bool IsLocked { get; set; }
    }
}
