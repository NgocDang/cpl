using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class SettingViewModel
    {
        public bool IsKYCVerificationActivated { get; set; }
        public bool IsAccountActivationEnable { get; set; }
        public int CookieExpirations { get; set; }

        public StandardAffiliateRateViewModel StandardAffiliateRate { get; set; }
        public AgencyAffiliateRateViewModel AgencyAffiliateRate { get; set; }
    }

    public class SettingDataModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
