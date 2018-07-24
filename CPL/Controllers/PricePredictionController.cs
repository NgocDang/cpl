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

            var btcCurrentPriceResult = ServiceClient.BTCCurrentPriceClient.GetBTCCurrentPriceAsync();
            btcCurrentPriceResult.Wait();

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

    }
}