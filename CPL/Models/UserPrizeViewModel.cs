using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class UserPrizeViewModel
    {
        public int LotteryId { get; set; }
        public int LotteryPrizeId { get; set; }
        public string Email { get; set; }
    }
}
