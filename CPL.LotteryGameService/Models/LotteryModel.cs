using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.LotteryGameService.Models
{
    public class LotteryModel
    {
        public int Id { get; set; }
        public int Phase { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Volume { get; set; }
        public int Status { get; set; }

        public List<LotteryHistoryModel> LotteryHistories { get; set; }
        public List<LotteryPrizeModel> LotteryPrizes { get; set; }
    }
}
