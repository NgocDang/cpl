using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class SecurityViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CurrentEmail { get; set; }
        public bool TwoFactorAuthenticationEnable { get; set; }
        public string QrCodeSetupImageUrl { get; set; }
    }
}
