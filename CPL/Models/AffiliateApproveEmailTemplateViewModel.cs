using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class AffiliateApproveEmailTemplateViewModel
    {
        public string RootUrl { get; set; }
        public TemplateViewModel Template { get; set; }

        public string AffiliateApplicationText { get; set; }
        public string HiText { get; set; }
        public string AffiliateApprovedDescriptionText { get; set; }
        public string CheersText { get; set; }
        public string ContactInfoText { get; set; }
        public string EmailText { get; set; }
        public string WebsiteText { get; set; }
        public string CPLTeamText { get; set; }
    }
}
