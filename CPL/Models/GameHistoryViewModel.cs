using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class GameHistoryViewModel
    {
        public int GameId { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Amount { get; set; }
        public bool? Result { get; set; }
        public decimal Balance { get; set; }
        public decimal? Bonus { get; set; }

        public string CreatedDateInString { get; set; }
        public string CreatedTimeInString { get; set; }
        public string AmountInString { get; set; }
        public string BonusInString { get; set; }
        public string BalanceInString { get; set; }
    }
}
