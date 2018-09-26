using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class StandardAffiliateAdminViewModel
    {
        public int Id { get; set; }
        public bool? KYCVerified { get; set; }
        public int? AffiliateId { get; set; }
        public string AffiliateUrl { get; set; }
        public int? AgencyId { get; set; }

        public bool IsKYCVerificationActivated { get; set; }

        public int TotalAffiliateSale { get; set; }
        public int TodayAffiliateSale { get; set; }
        public int YesterdayAfiliateSale { get; set; }

        public int TotalIntroducedUsers { get; set; }
        public int TotalIntroducedUserToday { get; set; }
        public int TotalIntroducedUsersYesterday { get; set; }

        public bool CanDoPayment { get; set; }

        public StandardAffiliateRateViewModel StandardAffiliateRate { get; set; }
        public AffiliateSettingViewModel StandardAffiliateSetting { get; set; }

        public string Tab { get; set; }

        public bool IsLocked { get; set; }
    }
}
