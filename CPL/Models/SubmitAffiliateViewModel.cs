using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class SubmitAffiliateViewModel
    {
        public bool? KYCVerified { get; set; }
        public int? AffiliateId { get; set; }
        public bool IsKYCVerificationActivated { get; set; }
    }
}
