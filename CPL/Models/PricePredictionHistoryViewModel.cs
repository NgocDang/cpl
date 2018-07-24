using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class PricePredictionHistoryViewModel
    {
        public int Id { get; set; }
        public DateTime PurcharseTime { get; set; }
        public string Bet { get; set; }
        public decimal StartRate { get; set; }
        public decimal Amount { get; set; }
        public decimal? Bonus { get; set; }
        public string Status { get; set; }
        public decimal? JudgmentRate{get;set;}
        public DateTime? JudgmentTime { get; set; }

        public string PurcharseTimeInString { get; set; }
        public string StartRateInString { get; set; }
        public string AmountInString { get; set; }
        public string BonusInString { get; set; }
        public string JudgmentRateInString { get; set; }
        public string JudgmentTimeInString { get; set; }

    }
}
