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
            this._templateService = templateService;
            this._sysUserService = sysUserService;
            this._pricePredictionService = pricePredictionService;
            this._pricePredictionHistoryService = pricePredictionHistoryService;
            this._quartzSchedulerService = quartzSchedulerService;
            this._btcPriceService = btcPriceService;
            this._progressHubContext = progressHubContext;
        }

        [Permission(EnumRole.Guest)]
        public IActionResult Index()
        {
            //Test quartz job price prediction update result
            // Cmt out because of new PricePrediction logic
            //var scheduler = _quartzSchedulerService.GetScheduler<IScheduler, IPricePredictionUpdateResultFactory>();
            //QuartzHelper.AddJob<PricePredictionUpdateResultJob>(scheduler, new DateTime(2018, 07, 30, 12, 56, 0));

            var viewModel = new PricePredictionIndexViewModel();
            viewModel.SysUserId = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser")?.Id;
            if (viewModel.SysUserId.HasValue)
                viewModel.TokenAmount = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id).TokenAmount;

            viewModel.PricePredictionTabs = _pricePredictionService.Queryable()
                .Where(x => x.ResultTime > DateTime.Now)
                .Select(x => Mapper.Map<PricePredictionTab>(x)).OrderBy(x => x.ResultTime.ToString("HH:mm"))
                .ToList();

            // Move first tab to the end of the array
            viewModel.PricePredictionTabs = Enumerable.Range(1, viewModel.PricePredictionTabs.Count).Select(i => viewModel.PricePredictionTabs[i % viewModel.PricePredictionTabs.Count]).ToList();

            if (viewModel.PricePredictionTabs.FirstOrDefault(x => x.CloseBettingTime >= DateTime.Now) != null)
                viewModel.PricePredictionTabs.FirstOrDefault(x => x.CloseBettingTime >= DateTime.Now).IsActive = true;

            return View(viewModel);
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
                                              ToBeComparedPrice = x.PricePrediction.ToBeComparedPrice,
                                              ToBeComparedPriceInString = x.PricePrediction.ToBeComparedPrice.GetValueOrDefault(0).ToString("#,##0.##"),
                                              ResultPrice = x.PricePrediction.ResultPrice,
                                              ResultPriceInString = $"{x.PricePrediction.ResultPrice.GetValueOrDefault(0).ToString("#,##0.##")} {EnumCurrency.USDT.ToString()}",
                                              ResultTime = x.PricePrediction.ResultTime,
                                              ResultTimeInString = x.PricePrediction.ResultTime.ToString(),
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
                                                    || x.ToBeComparedPriceInString.ToLower().Contains(searchBy)
                                                    || x.Status.ToLower().Contains(searchBy)
                                                    || x.AmountInString.ToLower().Contains(searchBy)
                                                    || x.BonusInString.ToLower().Contains(searchBy)
                                                    || x.ResultPriceInString.ToLower().Contains(searchBy)
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

                    decimal upPercentage;
                    decimal downPercentage;
                    //Calculate percentage
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
                    //////////////////////////


                    _progressHubContext.Clients.All.SendAsync("predictedUserProgress", upPercentage, downPercentage, pricePredictionId);


                    return new JsonResult(new { success = true, token = currentUser.TokenAmount.ToString("N0"), message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "BettingSuccessfully") });
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
        public IActionResult AddNewGame(PricePredictionIndexViewModel viewModel)
        {
            var user = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
            var currentUser = _sysUserService.Query().Select().FirstOrDefault(x => x.Id == user.Id && x.IsAdmin == true);
            if (currentUser != null)
            {
                // Update database

                // Add quartz job
                // Cmt out because of new PricePrediction logic
                //var scheduler = _quartzSchedulerService.GetScheduler<IScheduler, IPricePredictionUpdateResultFactory>();
                //QuartzHelper.AddJob<PricePredictionUpdateResultJob>(scheduler, viewModel.PredictionResultTime);

                return new EmptyResult();
            }
            else
                return new JsonResult(new { success = false, message = "You don't have permission to do this action" });
        }
    }
}