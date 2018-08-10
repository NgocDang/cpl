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
        public HoldingPercentageViewModel HoldingPercentage { get; set; }
        public List<WalletChangeViewModel> AssetChange { get; set; }
        public List<WalletChangeViewModel> MonthlyInvest { get; set; }
        public List<WalletChangeViewModel> BonusChange { get; set; }
        public List<GameHistoryViewModel> GameHistories { get; set; }
    }
}
