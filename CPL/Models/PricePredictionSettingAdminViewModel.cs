using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class PricePredictionSettingAdminViewModel
    {
        public int Id { get; set; }
        public TimeSpan OpenBettingTime { get; set; }
        public TimeSpan CloseBettingTime { get; set; }
        public int HoldingTimeInterval { get; set; }
        public int ResultTimeInterval { get; set; }
        public int DividendRate { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }

        public int PricePredictionCategoryId { get; set; }

        public string BettingTime { get; set; }
        public int HoldingTime { get; set; }
        public int RaffleTime { get; set; }
        public List<PricePredictionSettingDetailAdminViewModel> PricePredictionSettingDetails { get; set; }
    }
}
