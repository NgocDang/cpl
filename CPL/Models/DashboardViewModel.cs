using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class DashboardViewModel
    {
        public decimal TotalBalance { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool? KYCVerified { get; set; }
        public string KYCStatus { get; set; }
        
    }
}
