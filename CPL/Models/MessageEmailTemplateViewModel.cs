using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class MessageEmailTemplateViewModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string RootUrl { get; set; }
        public string PhoneNumber { get; set; }

        public string MessageFromCustomerText { get; set; }
        public string HiText { get; set; }
        public string NameText { get; set; }
        public string PhoneNumberText { get; set; }
        public string MessageText { get; set; }
        public string ContactInfoText { get; set; }
        public string EmailText { get; set; }
        public string WebsiteText { get; set; }
        public string CheersText { get; set; }
        public string CPLTeamText { get; set; }
    }
}
