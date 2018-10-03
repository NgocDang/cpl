using CPL.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CPL.Common.Enums.CPLConstant;

namespace CPL.Models
{
    public class AdminLotteryHistoryViewComponentViewModel
    {
        public int SysUserId { get; set; }
        public string UserName { get; set; }
        public int Status { get; set; }
        public int NumberOfTicket { get; set; }
        public decimal TotalPurchasePrice { get; set; }
        public string Title { get; set; }
        public DateTime PurchaseDateTime { get; set; }

        public string PurchaseDateTimeInString {
            get
            {
                return PurchaseDateTime.ToString(Format.DateTime);
            }
        }
        public string NumberOfTicketInString {
            get
            {
                return NumberOfTicket.ToString(Format.Number);
            }
        }

        public string StatusInString {
            get
            {
                return ((EnumLotteryGameStatus)(Status)).ToString();
            }
        }
    }
}
