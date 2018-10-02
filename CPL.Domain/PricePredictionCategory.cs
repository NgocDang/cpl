using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class PricePredictionCategory : Entity
    {
        public int Id { get; set; }

        public virtual ICollection<PricePredictionSetting> PricePredictionSettings { get; set; }
        public virtual ICollection<PricePredictionCategoryDetail> PricePredictionCategoryDetails { get; set; }
    }
}