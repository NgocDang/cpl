﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class LotteryIndexViewModel
    {
        public int Id { get; set; }
        public int Volume { get; set; }
        public List<LotteryHistoryViewModel> LotteryHistories { get; set; }

        public int? SysUserId { get; set; }
        public string PrecentOfPerchasedTickets { get; set; }
        public int UnitPrice { get; set; }
    }
}
