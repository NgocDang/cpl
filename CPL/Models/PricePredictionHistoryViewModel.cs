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
        public decimal? ToBeComparedPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal? Bonus { get; set; }
        public string Status { get; set; }
        public decimal? ResultPrice { get; set; }
        public DateTime? ResultTime { get; set; }

        public int PricePredictionId { get; set; }
        public int SysUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Prediction { get; set; }
        public string Result { get; set; }
        public decimal? Award { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string PurcharseTimeInString { get; set; }
        public string ToBeComparedPriceInString { get; set; }
        public string AmountInString { get; set; }
        public string BonusInString { get; set; }
        public string ResultPriceInString { get; set; }
        public string ResultTimeInString { get; set; }
    }
}
