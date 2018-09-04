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
        public int TotalNews { get; set; }

        public int TotalStandardAffiliate { get; set; }
        public int TotalStandardAffiliateToday { get; set; }
        public int TotalStandardAffiliateYesterday { get; set; }

        public int TotalAgencyAffiliate { get; set; }
        public int TotalAgencyAffiliateToday { get; set; }
        public int TotalAgencyAffiliateYesterday { get; set; }

        public string KYCVerificationActivated { get; set; }
        public string AccountActivationEnable { get; set; }

        public int Tier1StandardAffiliate { get; set; }
        public int Tier2StandardAffiliate { get; set; }
        public int Tier3StandardAffiliate { get; set; }

        public int AgencyDirectSaleTier1 { get; set; }
        public int AgencyDirectSaleTier2 { get; set; }
        public int AgencyDirectSaleTier3 { get; set; }
        public int AgencyTier2SaleToTier1 { get; set; }
        public int AgencyTier3SaleToTier1 { get; set; }
        public int AgencyTier3SaleToTier2 { get; set; }

        public int NumberOfExpiredDays { get; set; }
    }
}
