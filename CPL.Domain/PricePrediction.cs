using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class PricePrediction : Entity
    {
        public int Id { get; set; }
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
        public int PricePredictionSettingId { get; set; }
        public bool IsCreatedByAdmin { get; set; }
        public virtual PricePredictionSetting PricePredictionSetting { get; set; }
        public virtual ICollection<PricePredictionHistory> PricePredictionHistories { get; set; }
    }
}
