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
        public DateTime? UpdatedDate { get; set; }
        public int Volume { get; set; }
        public int Status { get; set; }
        public string Title { get; set; }
        public int UnitPrice { get; set; }
        public int LotteryCategoryId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<LotteryPrize> LotteryPrizes { get; set; }
        public virtual ICollection<LotteryHistory> LotteryHistories { get; set; }
        public virtual LotteryCategory LotteryCategory { get; set; }
        public virtual ICollection<LotteryDetail> LotteryDetails { get; set; }
    }
}