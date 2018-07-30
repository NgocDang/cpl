using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class LotteryHistoryViewModel
    {
        public int Id { get; set; }
        public int LotteryId { get; set; }
        public int SysUserId { get; set; }
        public int LotteryPhase { get; set; }
        public string LotteryPhaseInString { get; set; }
        public string Result { get; set; }
        public string TicketNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateInString { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateInString { get; set; }
        public decimal? Award { get; set; }
        public string AwardInString { get; set; }
        public int? LotteryPrizeId { get; set; }
        public int TicketIndex { get; set; }
    }
}
