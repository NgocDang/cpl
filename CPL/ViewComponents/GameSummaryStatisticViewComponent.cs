using CPL.Common.Enums;
using CPL.Core.Interfaces;
using CPL.Misc;
using CPL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.ViewComponents
{
    public class GameSummaryStatisticViewComponent : ViewComponent
    {
        private readonly ILotteryHistoryService _lotteryHistoryService;
        private readonly IPricePredictionHistoryService _pricePredictionHistoryService;
        private readonly IAnalyticService _analyticService;

        public GameSummaryStatisticViewComponent(
            ILotteryHistoryService lotteryHistoryService,
            IPricePredictionHistoryService pricePredictionHistoryService,
            IAnalyticService analyticService)
        {
            this._lotteryHistoryService = lotteryHistoryService;
            this._pricePredictionHistoryService = pricePredictionHistoryService;
            this._analyticService = analyticService;
        }

        public IViewComponentResult Invoke(double? periodInDay)
        {
            var viewModel = new GameSummaryStatisticViewComponentViewModel();
            var lotteryViews = _analyticService.GetPageViews(CPLConstant.Analytic.HomeViewId, periodInDay.GetValueOrDefault(0) > 0 ? DateTime.Now.AddDays(-periodInDay.GetValueOrDefault(0)) : CPLConstant.FirstDeploymentDate, DateTime.Now);
            var pricePredictionViews = _analyticService.GetPageViews(CPLConstant.Analytic.PricePredictionViewId, periodInDay.GetValueOrDefault(0) > 0 ? DateTime.Now.AddDays(-periodInDay.GetValueOrDefault(0)) : CPLConstant.FirstDeploymentDate, DateTime.Now);
            viewModel.PageView = lotteryViews.AsQueryable().Sum(x => x.Count) + pricePredictionViews.AsQueryable().Sum(x => x.Count);

            var lotteryTotalSale = _lotteryHistoryService.Query().Include(x => x.Lottery).Select()
                .Where(x => (periodInDay.GetValueOrDefault(0) > 0 ? x.CreatedDate.Date >= DateTime.Now.Date.AddDays(-periodInDay.GetValueOrDefault(0)) : x.CreatedDate <= DateTime.Now) && x.Result != EnumGameResult.REFUND.ToString())
                .Sum(x => x.Lottery.UnitPrice);
            var pricePredictionTotalSale = _pricePredictionHistoryService.Queryable()
                .Where(x => (periodInDay.GetValueOrDefault(0) > 0 ? x.CreatedDate.Date >= DateTime.Now.Date.AddDays(-periodInDay.GetValueOrDefault(0)) : x.CreatedDate <= DateTime.Now) && x.Result != EnumGameResult.REFUND.ToString())
                .Sum(x => x.Amount);
            viewModel.TotalSale = lotteryTotalSale + (int)pricePredictionTotalSale;
            viewModel.TodayPlayers = _lotteryHistoryService.Queryable().Where(x => x.CreatedDate.Date == DateTime.Now.Date).GroupBy(x => x.SysUserId).Count() + _pricePredictionHistoryService.Queryable().Where(x => x.CreatedDate.Date == DateTime.Now.Date).GroupBy(x => x.SysUserId).Count();
            var lotteryTotalPlayers = _lotteryHistoryService.Queryable()
                .Where(x => periodInDay.GetValueOrDefault(0) > 0 ? x.CreatedDate.Date >= DateTime.Now.Date.AddDays(-periodInDay.GetValueOrDefault(0)) : x.CreatedDate <= DateTime.Now)
                .Select(x => x.SysUserId);
            
            var pricePredictionTotalPlayers = _pricePredictionHistoryService.Queryable()
                .Where(x => periodInDay.GetValueOrDefault(0) > 0 ? x.CreatedDate.Date >= DateTime.Now.Date.AddDays(-periodInDay.GetValueOrDefault(0)) : x.CreatedDate <= DateTime.Now)
                .Select(x => x.SysUserId);

            viewModel.TotalPlayers = lotteryTotalPlayers.Concat(pricePredictionTotalPlayers).Distinct().Count();

            var lotteryTotalRevenue = _lotteryHistoryService.Query().Include(x => x.Lottery).Include(x => x.LotteryPrize).Select()
                .Where(x => (periodInDay.GetValueOrDefault(0) > 0 ? x.CreatedDate.Date >= DateTime.Now.Date.AddDays(-periodInDay.GetValueOrDefault(0)) : x.CreatedDate <= DateTime.Now) && x.Result != EnumGameResult.REFUND.ToString())
                .Sum(x => x.Lottery.UnitPrice - (x.LotteryPrizeId.HasValue ? x.LotteryPrize.Value : 0));
            var pricePredictionTotalRevenue = _pricePredictionHistoryService.Queryable()
                .Where(x => (periodInDay.GetValueOrDefault(0) > 0 ? x.CreatedDate.Date >= DateTime.Now.Date.AddDays(-periodInDay.GetValueOrDefault(0)) : x.CreatedDate <= DateTime.Now) && x.Result != EnumGameResult.REFUND.ToString())
                .Sum(x => x.Amount - x.Award.GetValueOrDefault(0));

            viewModel.TotalRevenue = Convert.ToInt32(lotteryTotalRevenue + pricePredictionTotalRevenue);
            return View(viewModel);
        }
    }
}
