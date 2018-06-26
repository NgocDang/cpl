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
    }
}
