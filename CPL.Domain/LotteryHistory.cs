using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class LotteryHistory : Entity
    {
        public int Id { get; set; }
        public int LotteryId { get; set; }
        public int SysUserId { get; set; }
        public string Result { get; set; }
        public string TicketNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? LotteryPrizeId { get; set; }
        public int TicketIndex { get; set; }


        public virtual Lottery Lottery { get; set; }
        public virtual SysUser SysUser { get; set; }
        public virtual LotteryPrize LotteryPrize { get; set; }
    }
}