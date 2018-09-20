using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class AllTopAgencyAffiliateViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? AffiliateId { get; set; }
        public int? AgencyId { get; set; }

        public int TotalIntroducer { get; set; }

        public DateTime? AffiliateCreatedDate { get; set; }
        public string Action { get; set; }
        public bool? IsLocked { get; set; }

        public decimal TotalSale { get; set; }
        public string TotalSaleInString { get; set; }

        // variable in string
        public string AffiliateCreatedDateInString { get; set; }

        public decimal Tier1DirectSale { get; set; }
        public decimal Tier2SaleToTier1Sale { get; set; }
        public decimal Tier3SaleToTier1Sale { get; set; }

        public int Tier1DirectRate { get; set; }
        public int Tier2DirectRate { get; set; }
        public int Tier3DirectRate { get; set; }
        public int Tier2SaleToTier1Rate { get; set; }
        public int Tier3SaleToTier1Rate { get; set; }
        public int Tier3SaleToTier2Rate { get; set; }
    }

    public class AllTopAgencyAffiliateDataModel
    {
        public int Id { get; set; }
        public int? Tier1DirectRate { get; set; }
        public int? Tier2DirectRate { get; set; }
        public int? Tier3DirectRate { get; set; }
        public int? Tier2SaleToTier1Rate { get; set; }
        public int? Tier3SaleToTier1Rate { get; set; }
        public int? Tier3SaleToTier2Rate { get; set; }

        public List<int> Ids { get; set; }
    }

}
