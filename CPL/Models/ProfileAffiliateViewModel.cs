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
        public int TotalSaleInToday { get; set; }
        public int TotalSaleInYesterday { get; set; }

        public int TotalUserRegister { get; set; }
        public int TotalUserRegisterInToday { get; set; }
        public int TotalUserRegisterInYesterday { get; set; }
    }
}
