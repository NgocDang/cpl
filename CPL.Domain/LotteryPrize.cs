using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class LotteryPrize: Entity
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public decimal Value { get; set; }
        public int LotteryId { get; set; }
        public int Volume { get; set; }

        public virtual ICollection<LotteryHistory> LotteryHistories { get; set; }
        public virtual Lottery Lottery { get; set; }
    }
}
