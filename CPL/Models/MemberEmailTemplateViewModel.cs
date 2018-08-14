using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class MemberEmailTemplateViewModel
    {
        public string Email { get; set; }
        public string RootUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public TemplateViewModel Template { get; set; }

        public string ActivationSuccessfulText { get; set; }
        public string HiText { get; set; }
        public string TeamMemberNowText { get; set; }
        public string PlayGameNowText { get; set; }
        public string CheersText { get; set; }
        public string ContactInfoText { get; set; }
        public string EmailText { get; set; }
        public string WebsiteText { get; set; }
        public string CPLTeamText { get; set; }
    }
}
