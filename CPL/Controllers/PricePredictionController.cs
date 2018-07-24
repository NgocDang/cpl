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
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using CPL.Hubs;
using CPL.Common.Enums;
using System;

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

            //// For testing
            //decimal upPrediction = 50;
            //decimal downPrediction = 50;
            //decimal upPercentage = Math.Round((upPrediction / (upPrediction + downPrediction) * 100), 2);
            //decimal downPercentage = 100 - upPercentage;

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
                .Queryable()
                .Where(x => x.SysUserId == user.Id)
                .Count();

            // search the dbase taking into consideration table sorting and paging
            var pricePredictionHistory = new List<PricePredictionHistoryViewModel>();
            if (string.IsNullOrEmpty(searchBy))
            {
                filteredResultsCount = totalResultsCount;

                pricePredictionHistory = _pricePredictionHistoryService
                        .Query()
                        .Include(x => x.PricePrediction)
                        .Select()
                        .Where(x => x.SysUserId == user.Id)
                        .Select(x => new PricePredictionHistoryViewModel
                        {
                            StartRate = x.PricePrediction.PredictionPrice,
                            StartRateInString = x.PricePrediction.PredictionPrice.ToString(),
                            JudgmentRate = x.PricePrediction.ResultPrice,
                            JudgmentRateInString = $"{x.PricePrediction.ResultPrice.ToString()} {EnumCurrency.USD.ToString()}",
                            JudgmentTime = x.PricePrediction.PredictionResultTime,
                            JudgmentTimeInString = x.PricePrediction.PredictionResultTime.ToString(),
                            Bet = x.Prediction == true ? EnumPricePredictionStatus.UP.ToString() : EnumPricePredictionStatus.DOWN.ToString(),
                            Status = x.UpdatedDate.HasValue == true ? EnumGameStatus.END.ToString() : EnumGameStatus.NOW.ToString(),
                            PurcharseTime = x.CreatedDate,
                            PurcharseTimeInString = $"{x.CreatedDate.ToString("yyyy/MM/dd hh:mm:ss")} {EnumCurrency.USD.ToString()}",
                            Bonus = x.Award.GetValueOrDefault(0),
                            BonusInString = $"{x.Award.GetValueOrDefault(0).ToString("#,##0.########")} {EnumCurrency.CPL.ToString()}",
                            Amount = x.Amount,
                            AmountInString = $"{x.Amount.ToString("#,##0.########")} {EnumCurrency.CPL.ToString()}",
                        })
                        .AsQueryable()
                        .Skip(skip)
                        .Take(take)
                        .OrderBy(sortBy, sortDir)
                        .ToList();
            }
            else
            {
                filteredResultsCount = _pricePredictionHistoryService
                    .Query()
                    .Include(x => x.PricePrediction)
                    .Select()
                    .Where(x => x.SysUserId == user.Id)
                    .Select(x => new PricePredictionHistoryViewModel
                    {
                        StartRate = x.PricePrediction.PredictionPrice,
                        StartRateInString = x.PricePrediction.PredictionPrice.ToString(),
                        JudgmentRate = x.PricePrediction.ResultPrice,
                        JudgmentRateInString = $"{x.PricePrediction.ResultPrice.ToString()} {EnumCurrency.USD.ToString()}",
                        JudgmentTime = x.PricePrediction.PredictionResultTime,
                        JudgmentTimeInString = x.PricePrediction.PredictionResultTime.ToString(),
                        Bet = x.Prediction == true ? EnumPricePredictionStatus.UP.ToString() : EnumPricePredictionStatus.DOWN.ToString(),
                        Status = x.UpdatedDate.HasValue == true ? EnumGameStatus.END.ToString() : EnumGameStatus.NOW.ToString(),
                        PurcharseTime = x.CreatedDate,
                        PurcharseTimeInString = $"{x.CreatedDate.ToString("yyyy/MM/dd hh:mm:ss")} {EnumCurrency.USD.ToString()}",
                        Bonus = x.Award.GetValueOrDefault(0),
                        BonusInString = $"{x.Award.GetValueOrDefault(0).ToString("#,##0.########")} {EnumCurrency.CPL.ToString()}",
                        Amount = x.Amount,
                        AmountInString = $"{x.Amount.ToString("#,##0.########")} {EnumCurrency.CPL.ToString()}",
                    })
                    .Where(x => x.PurcharseTimeInString.ToLower().Contains(searchBy) 
                                || x.Bet.ToLower().Contains(searchBy) 
                                || x.StartRateInString.ToLower().Contains(searchBy) 
                                || x.Status.ToLower().Contains(searchBy)
                                || x.AmountInString.ToLower().Contains(searchBy) 
                                || x.BonusInString.ToLower().Contains(searchBy) 
                                || x.JudgmentRateInString.ToLower().Contains(searchBy) 
                                || x.JudgmentTimeInString.ToLower().Contains(searchBy))
                    .Count();

                if (filteredResultsCount > 0)
                    pricePredictionHistory = _pricePredictionHistoryService
                        .Query()
                        .Include(x => x.PricePrediction)
                        .Select()
                        .Where(x => x.SysUserId == user.Id)
                        .Select(x => new PricePredictionHistoryViewModel
                        {
                            StartRate = x.PricePrediction.PredictionPrice,
                            StartRateInString = x.PricePrediction.PredictionPrice.ToString(),
                            JudgmentRate = x.PricePrediction.ResultPrice,
                            JudgmentRateInString = $"{x.PricePrediction.ResultPrice.ToString()} {EnumCurrency.USD.ToString()}",
                            JudgmentTime = x.PricePrediction.PredictionResultTime,
                            JudgmentTimeInString = x.PricePrediction.PredictionResultTime.ToString(),
                            Bet = x.Prediction == true ? EnumPricePredictionStatus.UP.ToString() : EnumPricePredictionStatus.DOWN.ToString(),
                            Status = x.UpdatedDate.HasValue == true ? EnumGameStatus.END.ToString() : EnumGameStatus.NOW.ToString(),
                            PurcharseTime = x.CreatedDate,
                            PurcharseTimeInString = $"{x.CreatedDate.ToString("yyyy/MM/dd hh:mm:ss")} {EnumCurrency.USD.ToString()}",
                            Bonus = x.Award.GetValueOrDefault(0),
                            BonusInString = $"{x.Award.GetValueOrDefault(0).ToString("#,##0.########")} {EnumCurrency.CPL.ToString()}",
                            Amount = x.Amount,
                            AmountInString = $"{x.Amount.ToString("#,##0.########")} {EnumCurrency.CPL.ToString()}",
                        })
                    .Where(x => x.PurcharseTimeInString.ToLower().Contains(searchBy)
                                || x.Bet.ToLower().Contains(searchBy)
                                || x.StartRateInString.ToLower().Contains(searchBy)
                                || x.Status.ToLower().Contains(searchBy)
                                || x.AmountInString.ToLower().Contains(searchBy)
                                || x.BonusInString.ToLower().Contains(searchBy)
                                || x.JudgmentRateInString.ToLower().Contains(searchBy)
                                || x.JudgmentTimeInString.ToLower().Contains(searchBy))
                    .AsQueryable()
                    .Skip(skip)
                    .Take(take)
                    .OrderBy(sortBy, sortDir)
                    .ToList();
            }

            return pricePredictionHistory.AsQueryable().OrderBy(sortBy, sortDir).ToList();
        }
    }
}