using AutoMapper;
using CPL.Common.Enums;
using CPL.Core.Interfaces;
using CPL.Infrastructure.Interfaces;
using CPL.Misc;
using CPL.Misc.Enums;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Controllers
{
    public class ViewComponentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly ISysUserService _sysUserService;
        private readonly IAnalyticService _analyticService;
        private readonly ILotteryHistoryService _lotteryHistoryService;
        private readonly IPricePredictionHistoryService _pricePredictionHistoryService;

        public ViewComponentController(
            IMapper mapper,
            IUnitOfWorkAsync unitOfWork,
            IAnalyticService analyticService,
            ILotteryHistoryService lotteryHistoryService,
            IPricePredictionHistoryService pricePredictionHistoryService,
            ISysUserService sysUserService)
        {
            this._mapper = mapper;
            this._sysUserService = sysUserService;
            this._lotteryHistoryService = lotteryHistoryService;
            this._pricePredictionHistoryService = pricePredictionHistoryService;
            this._analyticService = analyticService;
            this._unitOfWork = unitOfWork;
        }

        [Permission(EnumRole.User)]
        public IActionResult GetExchangeViewComponent()
        {
            return ViewComponent("Exchange");
        }

        [Permission(EnumRole.User)]
        public IActionResult GetRateViewComponent()
        {
            return ViewComponent("Rate");
        }

        [Permission(EnumRole.User)]
        public IActionResult GetDepositWithdrawViewComponent()
        {
            return ViewComponent("DepositWithdraw");
        }

        [Permission(EnumRole.Guest)]
        public IActionResult GetPricePredictionViewComponent(int id)
        {
            var viewModel = new PricePredictionViewComponentViewModel();
            viewModel.Id = id;
            viewModel.SysUserId = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser")?.Id;
            if (viewModel.SysUserId.HasValue)
            {
                viewModel.TokenAmount = _sysUserService.Queryable().FirstOrDefault(x => x.Id == viewModel.SysUserId).TokenAmount;
            }
            return ViewComponent("PricePrediction", viewModel);
        }

        [Permission(EnumRole.Admin)]
        public IActionResult GetSummaryStatisticsRevenueViewComponent(int periodInDay)
        {
            var viewModel = new PieChartViewComponentViewModel();
            var data = new List<PieChartData>();

            var totalSaleLotteryGame = _lotteryHistoryService.Query()
                                .Include(x => x.Lottery)
                                .Select(x => x.Lottery).Sum(y => y?.UnitPrice);
            var totalAwardLotteryGame = _lotteryHistoryService.Query()
                .Include(x => x.LotteryPrize)
                .Select(x => x.LotteryPrize).Sum(y => y?.Value);
            var revenueInLotteryGame = totalSaleLotteryGame - totalAwardLotteryGame;

            // price prediction game
            var totalSalePricePrediction = _pricePredictionHistoryService.Queryable()
                                                .Sum(x => x.Amount);
            var totalAwardPricePrediction = _pricePredictionHistoryService.Queryable()
                                                .Sum(x => x.Award);
            var revenueInPricePredictionGame = totalSalePricePrediction - totalAwardPricePrediction;

            var lotteryChartData = new PieChartData { Label = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Lottery"), Color = EnumHelper<EnumPieChartColor>.GetDisplayValue((EnumPieChartColor)1), Value = revenueInLotteryGame.GetValueOrDefault(0) };
            var pricePredictionChartData = new PieChartData { Label = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "PricePrediction"), Color = EnumHelper<EnumPieChartColor>.GetDisplayValue((EnumPieChartColor)2), Value = revenueInPricePredictionGame.GetValueOrDefault(0) };

            data.Add(lotteryChartData);
            data.Add(pricePredictionChartData);

            viewModel.ChartDataInJson = JsonConvert.SerializeObject(data, Formatting.Indented);
            viewModel.SeriesName = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Revenue");

            return ViewComponent("PieChart", viewModel);
        }

        [Permission(EnumRole.Admin)]
        public IActionResult GetSummaryStatisticsDeviceCategoryViewComponent(int periodInDay)
        {
            var viewModel = new PieChartViewComponentViewModel();

            var data = new List<PieChartData>();
            var deviceCategoriesLottery = _analyticService.GetDeviceCategory(CPLConstant.Analytic.HomeViewId, CPLConstant.FirstDeploymentDate, DateTime.Now);
            var totalDesktopLottery = deviceCategoriesLottery.Where(x => x.DeviceCategory == EnumDeviceCategory.DESKTOP).Sum(x => x.Count);
            var totalMobileLottery = deviceCategoriesLottery.Where(x => x.DeviceCategory == EnumDeviceCategory.MOBILE).Sum(x => x.Count);
            var totalTabletLottery = deviceCategoriesLottery.Where(x => x.DeviceCategory == EnumDeviceCategory.TABLET).Sum(x => x.Count);

            var deviceCategoriesPricePrediction = _analyticService.GetDeviceCategory(CPLConstant.Analytic.HomeViewId, CPLConstant.FirstDeploymentDate, DateTime.Now);
            var totalDesktopPricePrediction = deviceCategoriesPricePrediction.Where(x => x.DeviceCategory == EnumDeviceCategory.DESKTOP).Sum(x => x.Count);
            var totalMobilePricePrediction = deviceCategoriesPricePrediction.Where(x => x.DeviceCategory == EnumDeviceCategory.MOBILE).Sum(x => x.Count);
            var totalTabletPricePrediction = deviceCategoriesPricePrediction.Where(x => x.DeviceCategory == EnumDeviceCategory.TABLET).Sum(x => x.Count);

            var desktopChartData = new PieChartData { Label = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Desktop"), Color = EnumHelper<EnumPieChartColor>.GetDisplayValue((EnumPieChartColor)1), Value = totalDesktopLottery + totalDesktopPricePrediction };
            var mobileChartData = new PieChartData { Label = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Mobile"), Color = EnumHelper<EnumPieChartColor>.GetDisplayValue((EnumPieChartColor)2), Value = totalMobileLottery + totalMobilePricePrediction };
            var tabletChartData = new PieChartData { Label = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Tablet"), Color = EnumHelper<EnumPieChartColor>.GetDisplayValue((EnumPieChartColor)3), Value = totalTabletLottery + totalTabletPricePrediction };

            data.Add(desktopChartData);
            data.Add(mobileChartData);
            data.Add(tabletChartData);

            viewModel.ChartDataInJson = JsonConvert.SerializeObject(data, Formatting.Indented);
            viewModel.SeriesName = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Device");
            return ViewComponent("PieChart", viewModel);
        }
    }
}
