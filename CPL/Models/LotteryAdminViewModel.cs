using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class LotteryAdminViewModel
    {
        public LotteryAdminViewModel()
        {
            LotteryDetails = new List<LotteryDetailAdminViewModel>();
        }
        public int Id { get; set; }
        public int Phase { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Volume { get; set; }
        public int Status { get; set; }
        public string Title { get; set; }
        public int UnitPrice { get; set; }
        public int LotteryCategoryId { get; set; }
        public string LotteryCategory { get; set; }
        public string CreatedDateInString { get; set; }

        public bool IsPublished { get; set; }

        public List<LotteryHistoryViewModel> LotteryHistories { get; set; }
        public List<LotteryPrizeViewModel> LotteryPrizes { get; set; }
        public List<LotteryDetailAdminViewModel> LotteryDetails { get; set; }
        public List<LotteryCategoryAdminViewModel> LotteryCategories { get; set; }

    }
}
