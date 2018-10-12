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
using Microsoft.EntityFrameworkCore;
using CPL.Common.Misc;

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
        public IActionResult Index(bool? predictedTrend)
        {
            //Test quartz job price prediction update result
            // Cmt out because of new PricePrediction logic
            //var scheduler = _quartzSchedulerService.GetScheduler<IScheduler, IPricePredictionUpdateResultFactory>();
            //QuartzHelper.AddJob<PricePredictionUpdateResultJob>(scheduler, new DateTime(2018, 07, 30, 12, 56, 0));

            var viewModel = new PricePredictionIndexViewModel();
            viewModel.SysUserId = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser")?.Id;
            if (viewModel.SysUserId.HasValue)
                viewModel.TokenAmount = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id).TokenAmount;

            var adminPricePredictionTabs = _pricePredictionService.Queryable()
                .Where(x => x.ResultTime.Date == DateTime.Now.Date && x.PricePredictionCategoryId != (int)EnumPricePredictionCategory.SYSTEM)
                .Select(x => new PricePredictionTab
                {
                    Id = x.Id,
                    ResultTime = x.ResultTime,
                    ToBeComparedTime = x.ToBeComparedTime,
                    CloseBettingTime = x.CloseBettingTime,
                    IsDisabled = (x.OpenBettingTime > DateTime.Now || x.CloseBettingTime < DateTime.Now),
                    Title = x.PricePredictionDetails.FirstOrDefault(y => y.LangId == HttpContext.Session.GetInt32("LangId").Value).Title,
                    CoinBase = x.Coinbase
                });

            var systemPricePredictionTabs = _pricePredictionService.Queryable()
                .Where(x => x.ResultTime > DateTime.Now && x.PricePredictionCategoryId == (int)EnumPricePredictionCategory.SYSTEM)
                .Select(x => new PricePredictionTab
                {
                    Id = x.Id,
                    ResultTime = x.ResultTime,
                    ToBeComparedTime = x.ToBeComparedTime,
                    CloseBettingTime = x.CloseBettingTime,
                    IsDisabled = (x.CloseBettingTime < DateTime.Now),
                    Title = x.PricePredictionDetails.FirstOrDefault(y => y.LangId == HttpContext.Session.GetInt32("LangId").Value).Title,
                    CoinBase = x.Coinbase
                });         


            viewModel.PricePredictionTabs = adminPricePredictionTabs.Concat(systemPricePredictionTabs)
                .OrderBy(x => x.ResultTime.ToString("HH:mm"))
                .ToList();

            // Move first tab to the end of the array
            viewModel.PricePredictionTabs = Enumerable.Range(1, viewModel.PricePredictionTabs.Count).Select(i => viewModel.PricePredictionTabs[i % viewModel.PricePredictionTabs.Count]).ToList();

            var pricePredictionActiveTab = viewModel.PricePredictionTabs.FirstOrDefault(x => x.CloseBettingTime >= DateTime.Now);

            if (predictedTrend.HasValue)
            {
                if (viewModel.PricePredictionTabs.OrderBy(x => x.CloseBettingTime).FirstOrDefault(x => x.CloseBettingTime >= DateTime.Now) != null)
                {
                    viewModel.PricePredictionTabs.OrderBy(x => x.CloseBettingTime).FirstOrDefault(x => x.CloseBettingTime >= DateTime.Now).IsActive = true;
                    viewModel.PricePredictionTabs.OrderBy(x => x.CloseBettingTime).FirstOrDefault(x => x.CloseBettingTime >= DateTime.Now).PredictedTrend = predictedTrend;
                }
            }
            else
            {
                if (pricePredictionActiveTab != null)
                {
                    viewModel.PricePredictionTabs.FirstOrDefault(x => x.CloseBettingTime >= DateTime.Now).IsActive = true;
                    viewModel.PricePredictionTabs.FirstOrDefault(x => x.CloseBettingTime >= DateTime.Now).PredictedTrend = predictedTrend;
                }
            }

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

        [HttpPost]
        public IActionResult ConfirmPrediction(int pricePredictionId, decimal betAmount, bool predictedTrend)
        {
            var user = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
            var pricePrediction = _pricePredictionService.Queryable().FirstOrDefault(x => x.Id == pricePredictionId);
            if(DateTime.Now > pricePrediction.CloseBettingTime)
            {
                return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "OverBettingTime") });
            }
            else
            {
                if (user != null)
                {
                    var currentUser = _sysUserService.Queryable().FirstOrDefault(x => x.Id == user.Id);
                    if (betAmount < currentUser.TokenAmount)
                    {
                        var predictionRecord = new PricePredictionHistory() { PricePredictionId = pricePredictionId, Amount = betAmount, CreatedDate = DateTime.Now, Prediction = predictedTrend, SysUserId = user.Id };
                        _pricePredictionHistoryService.Insert(predictionRecord);

                        currentUser.TokenAmount -= betAmount;
                        _sysUserService.Update(currentUser);

                        _unitOfWork.SaveChanges();

                        decimal highPercentage;
                        decimal lowPercentage;
                        //Calculate percentage
                        decimal highPrediction = _pricePredictionHistoryService
                            .Queryable()
                            .Where(x => x.PricePredictionId == pricePredictionId && x.Prediction == EnumPricePredictionStatus.HIGH.ToBoolean() && x.Result != EnumGameResult.REFUND.ToString())
                            .Count();

                        decimal lowPrediction = _pricePredictionHistoryService
                            .Queryable()
                            .Where(x => x.PricePredictionId == pricePredictionId && x.Prediction == EnumPricePredictionStatus.LOW.ToBoolean() && x.Result != EnumGameResult.REFUND.ToString())
                            .Count();


                        if (highPrediction + lowPrediction == 0)
                        {
                            highPercentage = lowPercentage = 50;
                        }
                        else
                        {
                            highPercentage = Math.Round((highPrediction / (highPrediction + lowPrediction) * 100), 2);
                            lowPercentage = 100 - highPercentage;
                        }
                        //////////////////////////


                        _progressHubContext.Clients.All.SendAsync("predictedUserProgress", highPercentage, lowPercentage, pricePredictionId);


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
        }


        // Not implemented yet
        [HttpPost]
        public IActionResult AddNewGame(PricePredictionIndexViewModel viewModel)
        {
            var user = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
            var currentUser = _sysUserService.Queryable().FirstOrDefault(x => x.Id == user.Id && x.IsAdmin == true);
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