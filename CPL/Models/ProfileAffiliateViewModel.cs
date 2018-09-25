using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class ProfileAffiliateViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool? KYCVerified { get; set; }
        public int? AffiliateId { get; set; }
        public int? AgencyId { get; set; }
        public string AffiliateUrl { get; set; }

        public bool IsKYCVerificationActivated { get; set; }

        public int TotalSale { get; set; }
        public int TotalSaleToday { get; set; }
        public int TotalSaleYesterday { get; set; }

        public int TotalIntroducedUsers { get; set; }
        public int TotalIntroducedUsersToday { get; set; }
        public int TotalIntroducedUsersYesterday { get; set; }
    }
}
