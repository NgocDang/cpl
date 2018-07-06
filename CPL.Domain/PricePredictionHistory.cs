using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class PricePredictionHistory : Entity
    {
        public int Id { get; set; }
        public int PricePredictionId { get; set; }
        public int SysUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Amount { get; set; }
        public bool Prediction { get; set; }
        public string Result { get; set; }
        public decimal Award { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual PricePrediction PricePrediction { get; set; }
        public virtual SysUser SysUser { get; set; }
    }
}
