using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class TopAgencyAffiliateViewModel : ProfileAffiliateViewModel
    {
        public bool CanDoPayment { get; set; }

        public AgencyAffiliateRateViewModel AgencyAffiliateRate { get; set; }
        public AgencyAffiliateSettingViewModel AgencyAffiliateSetting { get; set; }

        public string Tab { get; set; }
    }
}
