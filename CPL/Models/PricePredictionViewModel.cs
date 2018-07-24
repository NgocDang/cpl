using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class PricePredictionViewModel
    {
        public int PricePredictionId { get; set; }
        public decimal UpPercentage { get; set; }
        public decimal DownPercentage { get; set; }
        public decimal CurrentBTCRate { get; set; }
        public string CurrentBTCRateInString { get; set; }
    }
}
