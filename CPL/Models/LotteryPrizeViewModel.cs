using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class LotteryPrizeViewModel
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public decimal Value { get; set; }
        public int LotteryId { get; set; }
        public int Volume { get; set; }

        public decimal Probability { get; set; }
    }
}
