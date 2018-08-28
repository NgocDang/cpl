using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class ActivateEmailTemplateViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ActivateToken { get; set; }
        public string ActivateUrl { get; set; }
        public string RootUrl { get; set; }
        public TemplateViewModel Template { get; set; }

        public string RegistrationSuccessfulText { get; set; }
        public string HiText { get; set; }
        public string RegisterActivateText { get; set; }
        public string ActivateText { get; set; }
        public string NotWorkUrlText { get; set; }
        public string CheersText { get; set; }
        public string ContactInfoText { get; set; }
        public string EmailText { get; set; }
        public string WebsiteText { get; set; }
        public string ExpiredEmail24hText { get; set; }
    }
}
