using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class ForgotPasswordEmailTemplateViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ResetPasswordToken { get; set; }
        public string RootUrl { get; set; }
        public string ResetPasswordUrl { get; set; }

        public string ResetYourPasswordText { get; set; }
        public string HiText { get; set; }
        public string ResetPasswordRequestText { get; set; }
        public string ButtonClickBelowText { get; set; }
        public string NotWorkUrlText { get; set; }
        public string NotYourRequestText { get; set; }
        public string CheersText { get; set; }
        public string ConnectWithUsText { get; set; }
        public string ContactInfoText { get; set; }
        public string EmailText { get; set; }
        public string WebsiteText { get; set; }
        public string ExpiredEmail24hText { get; set; }
    }
}
