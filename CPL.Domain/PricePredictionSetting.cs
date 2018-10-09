using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class PricePredictionSetting : Entity
    {
        public int Id { get; set; }
        public TimeSpan OpenBettingTime { get; set; }
        public TimeSpan CloseBettingTime { get; set; }
        public int HoldingTimeInterval { get; set; }
        public int ResultTimeInterval { get; set; }
        public int DividendRate { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public int PricePredictionCategoryId { get; set; }

        public virtual PricePredictionCategory PricePredictionCategory { get; set; }
        public virtual ICollection<PricePredictionSettingDetail> PricePredictionSettingDetails { get; set; }
    }
}
