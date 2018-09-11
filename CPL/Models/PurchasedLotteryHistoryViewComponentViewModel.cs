using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class PurchasedLotteryHistoryViewComponentViewModel
    {
        public string UserName { get; set; }
        public string Status { get; set; }
        public int NumberOfTicket { get; set; }
        public decimal TotalPurchasePrice { get; set; }
        public string Title { get; set; }
        public DateTime PurchaseDateTime { get; set; }
    }
}
