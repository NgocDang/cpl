using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class TopAgencyViewModel : ProfileAffiliateViewModel
    {
        public bool CanDoPayment { get; set; }

        public AgencyAffiliateRateViewModel AgencyAffiliateRate { get; set; }
        public TopAgencySettingViewModel TopAgencySetting { get; set; }
    }
}
