using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class ProfileViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string Mobile { get; set; }
        public bool? Gender { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public DateTime? DOB { get; set; }
        public bool? KYCVerified { get; set; }
        public DateTime? KYCCreatedDate { get; set; }
        public int? AffiliateId { get; set; }
        public string AffiliateStatus { get; set; }
        public string Email { get; set; }
        public string KYCStatus { get; set; }
        public string ETHHDWalletAddress { get; set; }
        public string BTCHDWalletAddress { get; set; }
        public bool TwoFactorAuthenticationEnable { get; set; }
        public string TwoFactorAuthenticationEnableStatus { get; set; }

        public int NumberOfTransactions { get; set; }
        public int NumberOfGameHistories { get; set; }
        public IFormFile FrontSideImage { set; get; }
        public IFormFile BackSideImage { set; get; }
    }
}
