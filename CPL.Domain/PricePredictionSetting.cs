using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class PricePredictionSetting : Entity
    {
        public int Id { get; set; }
        public DateTime OpenBettingTime { get; set; }
        public DateTime CloseBettingTime { get; set; }
        public int HoldingTimeInterval { get; set; }
        public int ResultTimeInterval { get; set; }
        public int DividedRate { get; set; }
        public DateTime CreatedDate { get; set; }

        public int PricePredictionCategoryId { get; set; }

        public virtual PricePredictionCategory PricePredictionCategory { get; set; }
        public virtual ICollection<PricePrediction> PricePredictions { get; set; }
        public virtual ICollection<PricePredictionSettingDetail> PricePredictionSettingDetails { get; set; }
    }
}
