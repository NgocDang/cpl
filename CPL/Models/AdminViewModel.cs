using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class AdminViewModel
    {
        public int TotalKYCVerified { get; set; }
        public int TotalKYCPending { get; set; }

        public int TotalUser { get; set; }

        public int TotalLotteryGame { get; set; }
        public int TotalLotteryGamePending { get; set; }
        public int TotalLotteryGameActive { get; set; }
        public int TotalLotteryGameCompleted { get; set; }
    }
}
