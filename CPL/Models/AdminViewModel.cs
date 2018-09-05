using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class AdminViewModel
    {
        public int TotalKYCVerified { get; set; }
        public int TotalKYCPending { get; set; }
        public int TotalUser { get; set; }
        public int TotalUserToday { get; set; }
        public int TotalUserYesterday { get; set; }

        public int TotalLotteryGame { get; set; }
        public int TotalLotteryGamePending { get; set; }
        public int TotalLotteryGameActive { get; set; }
        public int TotalLotteryGameCompleted { get; set; }

        public int TotalAffiliateApplicationApproved { get; set; }
        public int TotalAffiliateApplicationPending { get; set; }

        public int TotalStandardAffiliate { get; set; }
        public int TotalStandardAffiliateToday { get; set; }
        public int TotalStandardAffiliateYesterday { get; set; }

        public int TotalAgencyAffiliate { get; set; }
        public int TotalAgencyAffiliateToday { get; set; }
        public int TotalAgencyAffiliateYesterday { get; set; }

        public string KYCVerificationActivated { get; set; }
        public string AccountActivationEnable { get; set; }
        public int CookieExpirations { get; set; }

        public StandardAffiliateRateViewModel StandardAffiliate { get; set; }
        public AgencyAffiliateRateViewModel AgencyAffiliate { get; set; }

        public int NumberOfAgencyAffiliateExpiredDays { get; set; }
    }

    public class StandardAffiliateRateViewModel
    {
        public int Tier1DirectRate { get; set; }
        public int Tier2SaleToTier1Rate { get; set; }
        public int Tier3SaleToTier1Rate { get; set; }
    }

    public class AgencyAffiliateRateViewModel
    {
        public int Tier1DirectRate { get; set; }
        public int Tier2DirectRate { get; set; }
        public int Tier3DirectRate { get; set; }
        public int Tier2SaleToTier1Rate { get; set; }
        public int Tier3SaleToTier1Rate { get; set; }
        public int Tier3SaleToTier2Rate { get; set; }
    }
}
