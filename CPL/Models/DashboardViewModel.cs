using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class DashboardViewModel
    {
        public decimal TotalBalance { get; set; }
        public decimal ETHAmount { get; set; }
        public decimal BTCAmount { get; set; }
        public decimal TokenAmount { get; set; }
        public HoldingPercentageViewModel HoldingPercentage { get; set; }
        public List<WalletChangeViewModel> AssetChange { get; set; }
        public List<WalletChangeViewModel> MonthlyInvest { get; set; }
        public List<WalletChangeViewModel> BonusChange { get; set; }
        public List<GameHistoryViewModel> GameHistories { get; set; }
    }
}
