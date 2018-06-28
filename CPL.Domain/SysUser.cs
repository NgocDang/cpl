using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class SysUser : Entity
    {
        public int Id { get; set; }
        public bool IsAdmin { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string ETHWalletAddress { get; set; }
        public int BTCHDWalletAddressIndex { get; set; }
        public string BTCWalletAddress { get; set; }
        public string ETHHDWalletAddress { get; set; }
        public int ETHHDWalletAddressIndex { get; set; }
        public string BTCHDWalletAddress { get; set; }
        public string Mobile { get; set; }
        public DateTime? ResetPasswordDate { get; set; }
        public string ResetPasswordToken { get; set; }
        public string ActivateToken { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? KYCVerified { get; set; }
        public DateTime? KYCCreatedDate { get; set; }
        public string FrontSide { get; set; }
        public string BackSide { get; set; }
        public DateTime? DOB { get; set; }
        public bool IsDeleted { get; set; }
        public decimal BTCAmount { get; set; }
        public decimal ETHAmount { get; set; }
        public decimal TokenAmount { get; set; }

        public virtual ICollection<CoinTransaction> CoinTransactions { get; set; }
        public virtual ICollection<GameHistory> GameHistories { get; set; }
    }
}
