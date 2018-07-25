using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class Lottery : Entity
    {
        public int Id { get; set; }
        public int Phase { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Volume { get; set; }
        public int Status { get; set; }

        public virtual ICollection<LotteryPrize> LotteryPrizes { get; set; }
        public virtual ICollection<LotteryHistory> LotteryHistories { get; set; }
    }
}