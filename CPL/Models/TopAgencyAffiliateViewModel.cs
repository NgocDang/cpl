using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class TopAgencyAffiliateIntroducedUsersViewModel
    {
        public int sysUserId { get; set; }
        public decimal TotalCPLUsed { get; set; }
        public decimal TotalCPLAwarded { get; set; }
        public int TotalIntroducedUsers { get; set; }
        public DateTime? AffiliateCreatedDate { get; set; }
    }
}
