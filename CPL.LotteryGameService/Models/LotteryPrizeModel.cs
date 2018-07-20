using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.LotteryGameService.Models
{
    public class LotteryPrizeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public int LotteryId { get; set; }
        public int Volume { get; set; }
        public string Color { get; set; }
    }
}
