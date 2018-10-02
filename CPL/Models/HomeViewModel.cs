using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class HomeViewModel
    {
        public int? RandomLotteryId { get; set; }
        public int? RandomLotteryCategoryId { get; set; }
        public int? ClosestPricePredictionId { get; set; }
    }
}
