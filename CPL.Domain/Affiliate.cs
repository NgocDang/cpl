using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class Affiliate : Entity
    {
        public int Id { get; set; }
        public int Tier1DirectRate { get; set; }
        public int Tier2SaleToTier1Rate { get; set; }
        public int Tier3SaleToTier1Rate { get; set; }
        public bool IsAutoPaymentEnable { get; set; }
        public bool IsTier2TabVisible { get; set; }
        public bool IsTier3TabVisible { get; set; }

        public virtual SysUser SysUser { get; set; }
    }
}
