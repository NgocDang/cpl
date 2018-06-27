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

        // For multi languages
        public string RegistrationSuccessfulText { get; set; }
    }
}
