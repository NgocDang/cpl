using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class IntroducedUsers : Entity
    {
        public int Id { get; set; }
        public string DirectIntroducedUsers { get; set; }
        public int TotalDirectIntroducedUsers { get; set; }
        public string Tier2IntroducedUsers { get; set; }
        public int TotalTier2IntroducedUsers { get; set; }
        public string Tier3IntroducedUsers { get; set; }
        public int TotalTier3IntroducedUsers { get; set; }
        public decimal DirectAffiliateSale { get; set; }
        public decimal Tier2AffiliateSale { get; set; }
        public decimal Tier3AffiliateSale { get; set; }

        public virtual SysUser SysUser { get; set; }
    }
}
