using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class LotteryViewModel
    {
        public int Phase { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Volume { get; set; }

        public decimal FirstPrizeProbability { get; set; }
        public decimal SecondPrizeProbability { get; set; }
        public decimal ThirdPrizeProbability { get; set; }
        public decimal FourthPrizeProbability { get; set; }

        public int TotalTicket { get; set; }
        public int TicketCollected { get; set; }

        public int NumberOfTicketWinFirstPrize { get; set; }
        public int NumberOfTicketWinSecondPrize { get; set; }
        public int NumberOfTicketWinThirdPrize { get; set; }
        public int NumberOfTicketWinFourthPrize { get; set; }
    }
}
