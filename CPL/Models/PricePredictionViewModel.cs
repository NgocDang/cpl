using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class PricePredictionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime PredictionResultTime { get; set; }
        public decimal PredictionPrice { get; set; }
        public decimal? ResultPrice { get; set; }
        public int? NumberOfPredictors { get; set; }
        public decimal? Volume { get; set; }
        public string Coinbase { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public List<PricePredictionHistoryViewModel> PricePredictionHistories { get; set; }

        public int? PricePredictionId { get; set; }
        public decimal UpPercentage { get; set; }
        public decimal DownPercentage { get; set; }
        public decimal CurrentBTCRate { get; set; }
        public string CurrentBTCRateInString { get; set; }
        public int? SysUserId { get; set; }
        public string PreviousBtcRate { get; set; }
        public decimal LowestBtcRate { get; set; }
    }
}
