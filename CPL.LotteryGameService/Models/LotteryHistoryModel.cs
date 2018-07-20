﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.LotteryGameService.Models
{
    public class LotteryHistoryModel
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
    }
}
