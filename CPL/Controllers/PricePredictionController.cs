﻿using AutoMapper;
using CPL.Core.Interfaces;
using CPL.Infrastructure.Interfaces;
using CPL.Misc;
using CPL.Misc.Enums;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using CPL.Hubs;
using CPL.Common.Enums;
using System;
using CPL.Domain;

namespace CPL.Controllers
{
    [Permission(EnumRole.Guest)]
    public class PricePredictionController : Controller
    {
        private readonly ILangService _langService;
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly ISettingService _settingService;
        private readonly IGameHistoryService _gameHistoryService;
        private readonly ITeamService _teamService;
        private readonly ITemplateService _templateService;
        private readonly ISysUserService _sysUserService;
        private readonly IPricePredictionService _pricePredictionService;
        private readonly IPricePredictionHistoryService _pricePredictionHistoryService;
        private readonly IHubContext<UserPredictionProgressHub> _progressHubContext;


        public PricePredictionController(
            ILangService langService,
            IMapper mapper,
            IViewRenderService viewRenderService,
            IUnitOfWorkAsync unitOfWork,
            ISettingService settingService,
            ITeamService teamService,
            ITemplateService templateService,
            ISysUserService sysUserService,
            IGameHistoryService gameHistoryService,
            IPricePredictionService pricePredictionService,
            IPricePredictionHistoryService pricePredictionHistoryService,
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
            this._gameHistoryService = gameHistoryService;
            this._pricePredictionService = pricePredictionService;
            this._pricePredictionHistoryService = pricePredictionHistoryService;
            this._progressHubContext = progressHubContext;
        }

        public IActionResult Index()
        {
            var viewModel = new PricePredictionViewModel();
            int? currentGameId = _pricePredictionService.Query().Select().FirstOrDefault(x => x.ResultPrice == null)?.Id;
            decimal upPercentage;
            decimal downPercentage;
            this.CalculatePercentagePrediction(currentGameId.GetValueOrDefault(0), out upPercentage, out downPercentage);

            var btcCurrentPriceResult = ServiceClient.BTCCurrentPriceClient.GetBTCCurrentPriceAsync();
            btcCurrentPriceResult.Wait();

            viewModel.SysUserId = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser")?.Id;
            if (btcCurrentPriceResult.Result.Status.Code == 0)
            {
                viewModel.CurrentBTCRate = btcCurrentPriceResult.Result.Price;
                viewModel.CurrentBTCRateInString = btcCurrentPriceResult.Result.Price.ToString("#,##0.00");
            }
                
            // Set to Model
            viewModel.PricePredictionId = 1;
            viewModel.UpPercentage = upPercentage;
            viewModel.DownPercentage = downPercentage;

            return View(viewModel);
        }

        [HttpPost]
        public void PredictResult(PricePredictionResponseViewModel viewModel)
        {
            decimal upPercentage;
            decimal downPercentage;
            this.CalculatePercentagePrediction(viewModel.PricePredictionId, out upPercentage, out downPercentage);
            // PROGRESS
            _progressHubContext.Clients.All.SendAsync("predictedUserProgress", upPercentage, downPercentage);
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
            } else
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
                    return new JsonResult(new { success = true, value = btcCurrentPriceResult.Result.Price, valueInString = btcCurrentPriceResult.Result.Price.ToString("#,##0.00") });

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
                sortDir = model.order[0].dir.ToLower() == "asc";
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
                                              StartRateInString = x.PricePrediction.PredictionPrice.ToString(),
                                              ResultRate = x.PricePrediction.ResultPrice,
                                              ResultRateInString = $"{x.PricePrediction.ResultPrice.ToString()} {EnumCurrency.USD.ToString()}",
                                              ResultTime = x.PricePrediction.PredictionResultTime,
                                              ResultTimeInString = x.PricePrediction.PredictionResultTime.ToString(),
                                              Bet = x.Prediction == true ? EnumPricePredictionStatus.UP.ToString() : EnumPricePredictionStatus.DOWN.ToString(),
                                              Status = x.UpdatedDate.HasValue == true ? EnumLotteryGameStatus.COMPLETED.ToString() : EnumLotteryGameStatus.ACTIVE.ToString(),
                                              PurcharseTime = x.CreatedDate,
                                              PurcharseTimeInString = $"{x.CreatedDate.ToString("yyyy/MM/dd hh:mm:ss")} {EnumCurrency.USD.ToString()}",
                                              Bonus = x.Award.GetValueOrDefault(0),
                                              BonusInString = $"{x.Award.GetValueOrDefault(0).ToString("#,##0.########")} {EnumCurrency.CPL.ToString()}",
                                              Amount = x.Amount,
                                              AmountInString = $"{x.Amount.ToString("#,##0.########")} {EnumCurrency.CPL.ToString()}",
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
        public IActionResult ConfirmPrediction(decimal betAmount, bool predictedTrend)
        {
            var user = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
            int? currentGameId = _pricePredictionService.Query().Select().LastOrDefault(x => x.ResultPrice == null)?.Id;

            if (user != null && currentGameId.HasValue)
            {
                var currentUser = _sysUserService.Query().Select().FirstOrDefault(x => x.Id == user.Id);
                if (betAmount < currentUser.TokenAmount)
                {
                    var predictionPrice = _pricePredictionService.Queryable().Where(x => x.Id == currentGameId).FirstOrDefault().PredictionPrice;
                    var predictionRecord = new PricePredictionHistory() { PricePredictionId = currentGameId.Value, Amount = betAmount, CreatedDate = DateTime.Now, Prediction = predictedTrend, SysUserId = user.Id };
                    _pricePredictionHistoryService.Insert(predictionRecord);
                    
                    currentUser.TokenAmount -= betAmount;
                    _sysUserService.Update(currentUser);

                    _unitOfWork.SaveChanges();
                    return new JsonResult(new { success = true });
                }
                return new JsonResult(new { success = false, message = "Insufficient funds!" });

            }

            return new JsonResult(new { success = false, message = "LogIn" });
        }
    }
}