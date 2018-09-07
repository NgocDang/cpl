using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class PricePredictionViewComponentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime OpenBettingTime { get; set; }
        public DateTime CloseBettingTime { get; set; }
        public DateTime ResultTime { get; set; }
        public DateTime ToBeComparedTime { get; set; }
        public decimal? ResultPrice { get; set; }
        public decimal? ToBeComparedPrice { get; set; }
        public int? NumberOfPredictors { get; set; }
        public decimal? Volume { get; set; }
        public string Coinbase { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public decimal UpPercentage { get; set; }
        public decimal DownPercentage { get; set; }
        public decimal CurrentBTCRate { get; set; }
        public string CurrentBTCRateInString { get; set; }
        public int? SysUserId { get; set; }
        public decimal? TokenAmount { get; set; }
        public string PreviousBtcRate { get; set; }
        public decimal LowestBtcRate { get; set; }

        public List<PricePredictionHistoryViewModel> PricePredictionHistories { get; set; }
    }
}
