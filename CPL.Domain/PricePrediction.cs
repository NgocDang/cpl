using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class PricePrediction : Entity
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

        public virtual ICollection<PricePredictionHistory> PricePredictionHistories { get; set; }
    }
}
