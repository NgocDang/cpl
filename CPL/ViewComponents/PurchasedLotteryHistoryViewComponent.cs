using CPL.Common.Enums;
using CPL.Core.Interfaces;
using CPL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CPL.Common.Enums.CPLConstant;

namespace CPL.ViewComponents
{
    public class PurchasedLotteryHistoryViewComponent : ViewComponent
    {
        private readonly ILotteryCategoryService _lotteryCategoryService;
        private readonly ILotteryService _lotteryService;
        private readonly ILotteryHistoryService _lotteryHistoryService;



        public PurchasedLotteryHistoryViewComponent(
            ILotteryCategoryService lotteryCategoryService,
            ILotteryService lotteryService,
            ILotteryHistoryService lotteryHistoryService)
        {
            this._lotteryCategoryService = lotteryCategoryService;
            this._lotteryService = lotteryService;
            this._lotteryHistoryService = lotteryHistoryService;
        }

        public IViewComponentResult Invoke(int? lotteryCategoryId)
        {
            var lotteryCategory = _lotteryCategoryService.Queryable().FirstOrDefault(x => x.Id == lotteryCategoryId);
            var purchasedLotteryHistory = _lotteryHistoryService
                .Query()
                .Include(x => x.Lottery)
                .Include(x => x.SysUser)
                .Select()
                .Where(x => !lotteryCategoryId.HasValue || x.Lottery.LotteryCategoryId == lotteryCategoryId)
                .GroupBy(x => new { x.CreatedDate , x.LotteryId, x.SysUser.Email })
                .Select(y => new PurchasedLotteryHistoryViewComponentViewModel
                {
                    UserName = y.Key.Email,
                    Status = ((EnumLotteryGameStatus)(y.FirstOrDefault().Lottery.Status)).ToString(),
                    NumberOfTicket = y.Count(),
                    TotalPurchasePrice = y.Sum(x => x.Lottery.UnitPrice),
                    Title = y.FirstOrDefault().Lottery.Title,
                    PurchaseDateTime = y.Key.CreatedDate
                }).ToList();

            return View();
        }
    }
}
