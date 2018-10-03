using CPL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class HomeViewModel
    {
        public int? RandomLotteryId { get; set; }
        public int RandomLotteryCategoryId { get; set; }
        public string RandomLotteryTitle { get; set; }
        public string RandomLotteryDescription { get; set; }
        public int? ClosestPricePredictionId { get; set; }
        public string ClosestPricePredictionTitle { get; set; }
        public string ClosestPricePredictionDescription { get; set; }
        public List<FAQViewModel> FAQs { get; set; }
    }
}
