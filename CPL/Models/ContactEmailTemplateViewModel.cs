using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class ContactEmailTemplateViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public string Email { get; set; }
        public string RootUrl { get; set; }
        public string CategoryName { get; set; }

        public string MessageFromCustomerText { get; set; }
        public string HiText { get; set; }
        public string SubjectText { get; set; }
        public string CategoryText { get; set; }
        public string DescriptionText { get; set; }
        public string ContactInfoText { get; set; }
        public string EmailText { get; set; }
        public string WebsiteText { get; set; }
        public string CheersText { get; set; }
        public string CPLTeamText { get; set; }
    }
}
