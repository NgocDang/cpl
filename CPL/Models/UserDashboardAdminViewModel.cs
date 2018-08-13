using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class UserDashboardAdminViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool TwoFactorAuthenticationEnable { get; set; }
        public string StreetAddress { get; set; }
        public string Mobile { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public bool? KYCVerified { get; set; }
        public string Address { get; set; }

        public string BTCHDWalletAddress { get; set; }
        public string ETHHDWalletAddress { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal ETHAmount { get; set; }
        public decimal BTCAmount { get; set; }
        public decimal TokenAmount { get; set; }
    }
}
