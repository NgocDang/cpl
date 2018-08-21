using AutoMapper;
using CPL.Core.Interfaces;
using CPL.Infrastructure.Interfaces;
using CPL.Misc;
using CPL.Misc.Enums;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using CPL.Hubs;
using CPL.Common.Enums;
using System;
using CPL.Domain;
using Quartz;
using CPL.Misc.Quartz;
using CPL.Misc.Quartz.Jobs;
using CPL.Misc.Quartz.Interfaces;

namespace CPL.Controllers
{
    public class PricePredictionController : Controller
    {
        private readonly ILangService _langService;
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly ISettingService _settingService;
        private readonly ITeamService _teamService;
        private readonly ITemplateService _templateService;
        private readonly ISysUserService _sysUserService;
        private readonly IPricePredictionService _pricePredictionService;
        private readonly IPricePredictionHistoryService _pricePredictionHistoryService;
        private readonly IBTCPriceService _btcPriceService;
        private readonly IHubContext<UserPredictionProgressHub> _progressHubContext;
        private readonly IQuartzSchedulerService _quartzSchedulerService;

        public PricePredictionController(
            ILangService langService,
            IMapper mapper,
            IViewRenderService viewRenderService,
            IUnitOfWorkAsync unitOfWork,
            ISettingService settingService,
            ITeamService teamService,
            ITemplateService templateService,
            ISysUserService sysUserService,
            IPricePredictionService pricePredictionService,
            IPricePredictionHistoryService pricePredictionHistoryService,
            IQuartzSchedulerService quartzSchedulerService,
            IBTCPriceService btcPriceService,
            IHubContext<UserPredictionProgressHub> progressHubContext)
        {
            this._langService = langService;
            this._mapper = mapper;
            this._viewRenderService = viewRenderService;
            this._settingService = settingService;
            this._unitOfWork = unitOfWork;
            this._teamService = teamService;
            this._templateService = templateService;
            this._sysUserService = sysUserService;
            this._pricePredictionService = pricePredictionService;
            this._pricePredictionHistoryService = pricePredictionHistoryService;
            this._quartzSchedulerService = quartzSchedulerService;
            this._btcPriceService = btcPriceService;
            this._progressHubContext = progressHubContext;
        }

        public IActionResult Index()
        {
            // Test quartz job price prediction update result
            //var scheduler = _quartzSchedulerService.GetScheduler<IScheduler, IPricePredictionUpdateResultFactory>();
            //QuartzHelper.AddJob<PricePredictionUpdateResultJob>(scheduler, new DateTime(2018, 07, 30, 12, 56, 0));

            //var viewModel = new PricePredictionViewModel();
            //viewModel.PricePredictionId = _pricePredictionService.Queryable().LastOrDefault(x => !x.UpdatedDate.HasValue)?.Id;

            //if (viewModel.PricePredictionId.HasValue)
            //{
            //    decimal upPercentage;
            //    decimal downPercentage;
            //    this.CalculatePercentagePrediction(viewModel.PricePredictionId.Value, out upPercentage, out downPercentage);
            //    // Set to Model
            //    viewModel.UpPercentage = upPercentage;
            //    viewModel.DownPercentage = downPercentage;
            //}

            //var btcCurrentPriceResult = ServiceClient.BTCCurrentPriceClient.GetBTCCurrentPriceAsync();
            //btcCurrentPriceResult.Wait();
            //if (btcCurrentPriceResult.Result.Status.Code == 0)
            //{
            //    viewModel.CurrentBTCRate = btcCurrentPriceResult.Result.Price;
            //    viewModel.CurrentBTCRateInString = btcCurrentPriceResult.Result.Price.ToString("#,##0.00");
            //}

            //// Get btc previous rates 12h before until now
            //var btcPriceInLocals = _btcPriceService.Queryable().Where(x => x.Time >= ((DateTimeOffset)DateTime.UtcNow.AddHours(-CPLConstant.HourBeforeInChart)).ToUnixTimeSeconds())
            //    .GroupBy(x => x.Time)
            //    .Select(y => new PricePredictionHighChartViewModel
            //    {
            //        Time = y.Key,
            //        Price = y.Select(x => x.Price).OrderByDescending(x => x).FirstOrDefault()
            //    })
            //    .ToList();

            //var currentTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
            //var listCurrentTime = new Dictionary<long, decimal>();
            //var second = CPLConstant.HourBeforeInChart * 60 * 60 - 1; // currently 43200
            //for (int i = -second; i <= 0; i++)
            //{
            //    listCurrentTime.Add(currentTime + i, 0); // Default Price is 0;
            //}

            //// Join 2 list
            //var pricePredictionViewModels = (from left in listCurrentTime.Keys
            //                                 join right in btcPriceInLocals on left equals right.Time into leftRight
            //                                 from lr in leftRight.DefaultIfEmpty()
            //                                 select new PricePredictionHighChartViewModel
            //                                 {
            //                                     Time = left,
            //                                     Price = lr?.Price,
            //                                 })
            //                                .ToList();

            //decimal value = 0;
            //for (int i = 0; i < pricePredictionViewModels.Count; i++)
            //{
            //    if (pricePredictionViewModels[i].Price != null)
            //    {
            //        value = pricePredictionViewModels[i].Price.GetValueOrDefault(0);
            //    }

            //    pricePredictionViewModels[i].Price = value;
            //}

            //var previousTime = pricePredictionViewModels.FirstOrDefault().Time.ToString();
            //var previousRate = string.Join(",", pricePredictionViewModels.Select(x => x.Price));
            //var lowestRate = pricePredictionViewModels.Where(x => x.Price != 0).Min(x => x.Price).GetValueOrDefault(0) - CPLConstant.LowestRateBTCInterval;
            //if (lowestRate < 0)
            //    lowestRate = 0;
            //var previousBtcRate = $"{previousTime};{previousRate}";

            //viewModel.PreviousBtcRate = previousBtcRate;
            //viewModel.LowestBtcRate = lowestRate;

            //// Get history game
            //viewModel.SysUserId = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser")?.Id;

            //return View(viewModel);

            return View();
        }

        private void CalculatePercentagePrediction(int pricePredictionId, out decimal upPercentage, out decimal downPercentage)
        {
            decimal upPrediction = _pricePredictionHistoryService
                .Queryable()
                .Where(x => x.PricePredictionId == pricePredictionId && x.Prediction == EnumPricePredictionStatus.UP.ToBoolean())
                .Count();

            decimal downPrediction = _pricePredictionHistoryService
                .Queryable()
                .Where(x => x.PricePredictionId == pricePredictionId && x.Prediction == EnumPricePredictionStatus.DOWN.ToBoolean())
                .Count();

            if (upPrediction + downPrediction == 0)
            {
                upPercentage = downPercentage = 50;
            }
            else
            {
                upPercentage = Math.Round((upPrediction / (upPrediction + downPrediction) * 100), 2);
                downPercentage = 100 - upPercentage;
            }
        }

        [HttpPost]
        public IActionResult GetBTCCurrentRate()
        {
            try
            {
                var btcCurrentPriceResult = ServiceClient.BTCCurrentPriceClient.GetBTCCurrentPriceAsync();
                btcCurrentPriceResult.Wait();

                if (btcCurrentPriceResult.Result.Status.Code == 0)
                    return new JsonResult(new { success = true, value = btcCurrentPriceResult.Result.Price, valueInString = $"{btcCurrentPriceResult.Result.Price.ToString("#,##0.00")};{btcCurrentPriceResult.Result.Price.ToString()};{btcCurrentPriceResult.Result.DateTime.ToString()}" });

                return new JsonResult(new { success = false, value = 0, valueInString = "0" });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public JsonResult SearchPricePredictionHistory(DataTableAjaxPostModel viewModel)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchPricePredictionHistoryFunc(viewModel, out filteredResultsCount, out totalResultsCount);
            return Json(new
            {
                // this is what datatables wants sending back
                draw = viewModel.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res
            });
        }

        public IList<PricePredictionHistoryViewModel> SearchPricePredictionHistoryFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var user = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
            var searchBy = (model.search != null) ? model.search.value?.ToLower() : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            bool sortDir = true;

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "desc";
            }

            totalResultsCount = _pricePredictionHistoryService
                                 .Query()
                                 .Include(x => x.PricePrediction)
                                 .Select()
                                 .Where(x => x.SysUserId == user.Id)
                                 .Count();

            // search the dbase taking into consideration table sorting and paging
            var pricePredictionHistory = _pricePredictionHistoryService
                                          .Query()
                                          .Include(x => x.PricePrediction)
                                          .Select()
                                          .Where(x => x.SysUserId == user.Id)
                                          .Select(x => new PricePredictionHistoryViewModel
                                          {
                                              StartRate = x.PricePrediction.PredictionPrice,
                                              StartRateInString = x.PricePrediction.PredictionPrice.ToString("#,##0.##"),
                                              ResultRate = x.PricePrediction.ResultPrice,
                                              ResultRateInString = $"{x.PricePrediction.ResultPrice.GetValueOrDefault(0).ToString("#,##0.##")} {EnumCurrency.USDT.ToString()}",
                                              ResultTime = x.PricePrediction.PredictionResultTime,
                                              ResultTimeInString = x.PricePrediction.PredictionResultTime.ToString(),
                                              Bet = x.Prediction == true ? EnumPricePredictionStatus.UP.ToString() : EnumPricePredictionStatus.DOWN.ToString(),
                                              Status = x.UpdatedDate.HasValue == true ? EnumLotteryGameStatus.COMPLETED.ToString() : EnumLotteryGameStatus.ACTIVE.ToString(),
                                              PurcharseTime = x.CreatedDate,
                                              PurcharseTimeInString = $"{x.CreatedDate.ToString("yyyy/MM/dd hh:mm:ss")}",
                                              Bonus = x.Award.GetValueOrDefault(0),
                                              BonusInString = $"{x.Award.GetValueOrDefault(0).ToString("#,##0.##")} {EnumCurrency.CPL.ToString()}",
                                              Amount = x.Amount,
                                              AmountInString = $"{x.Amount.ToString("#,##0.##")} {EnumCurrency.CPL.ToString()}",
                                          });

            if (string.IsNullOrEmpty(searchBy))
            {
                filteredResultsCount = totalResultsCount;
            }
            else
            {
                pricePredictionHistory = pricePredictionHistory
                                        .Where(x => x.PurcharseTimeInString.ToLower().Contains(searchBy)
                                                    || x.Bet.ToLower().Contains(searchBy)
                                                    || x.StartRateInString.ToLower().Contains(searchBy)
                                                    || x.Status.ToLower().Contains(searchBy)
                                                    || x.AmountInString.ToLower().Contains(searchBy)
                                                    || x.BonusInString.ToLower().Contains(searchBy)
                                                    || x.ResultRateInString.ToLower().Contains(searchBy)
                                                    || x.ResultTimeInString.ToLower().Contains(searchBy));

                filteredResultsCount = pricePredictionHistory.Count();
            }

            return pricePredictionHistory.AsQueryable().OrderBy(sortBy, sortDir).Skip(skip).Take(take).ToList();
        }

        [HttpPost]
        public IActionResult ConfirmPrediction(int pricePredictionId, decimal betAmount, bool predictedTrend)
        {
            var user = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
            if (user != null)
            {
                var currentUser = _sysUserService.Query().Select().FirstOrDefault(x => x.Id == user.Id);
                if (betAmount < currentUser.TokenAmount)
                {
                    var predictionRecord = new PricePredictionHistory() { PricePredictionId = pricePredictionId, Amount = betAmount, CreatedDate = DateTime.Now, Prediction = predictedTrend, SysUserId = user.Id };
                    _pricePredictionHistoryService.Insert(predictionRecord);

                    currentUser.TokenAmount -= betAmount;
                    _sysUserService.Update(currentUser);

                    _unitOfWork.SaveChanges();

                    // Signify up and down percentage
                    decimal upPercentage;
                    decimal downPercentage;
                    this.CalculatePercentagePrediction(pricePredictionId, out upPercentage, out downPercentage);
                    _progressHubContext.Clients.All.SendAsync("predictedUserProgress", upPercentage, downPercentage);


                    return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "BettingSuccessfully") });
                }
                return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "InsufficientFunds") });
            }
            return new JsonResult(new
            {
                success = true,
                url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Action("LogIn", "Authentication")}?returnUrl={Url.Action("Index", "PricePrediction")}"
            });
        }


        // Not implemented yet
        [HttpPost]
        public IActionResult AddNewGame(PricePredictionViewModel viewModel)
        {
            var user = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
            var currentUser = _sysUserService.Query().Select().FirstOrDefault(x => x.Id == user.Id && x.IsAdmin == true);
            if (currentUser != null)
            {
                // Update database

                // Add quartz job
                var scheduler = _quartzSchedulerService.GetScheduler<IScheduler, IPricePredictionUpdateResultFactory>();
                QuartzHelper.AddJob<PricePredictionUpdateResultJob>(scheduler, viewModel.PredictionResultTime);

                return new EmptyResult();
            }
            else
                return new JsonResult(new { success = false, message = "You don't have permission to do this action" });
        }
    }
}