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
        private readonly IPricePredictionHistoryService _pricePredictionHistoryService;
        private readonly IHubContext<ProgressHub> _progressHubContext;


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
            IPricePredictionHistoryService pricePredictionHistoryService,
            IHubContext<ProgressHub> progressHubContext)
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
            this._pricePredictionHistoryService = pricePredictionHistoryService;
            this._progressHubContext = progressHubContext;
        }

        public IActionResult Index()
        {
            var viewModel = new PricePredictionViewModel();
            int? currentGameId = _pricePredictionHistoryService
                .Query()
                .Include(x => x.PricePrediction)
                .Select()
                .FirstOrDefault(x => x.PricePrediction.ResultPrice == null)?.Id;
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
        public void PredictResult(PricePredictionResViewModel viewModel)
        {
            decimal upPercentage;
            decimal downPercentage;
            this.CalculatePercentagePrediction(viewModel.PricePredictionId, out upPercentage, out downPercentage);
            // For testing
            //decimal upPrediction = 13;
            //decimal downPrediction = 26;
            //decimal upPercentage = Math.Round((upPrediction / (upPrediction + downPrediction) * 100), 2);
            //decimal downPercentage = 100 - upPercentage;

            // PROGRESS
            _progressHubContext.Clients.All.SendAsync("preditedUserProgress", upPercentage, downPercentage);
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

            upPercentage = Math.Round((upPrediction / (upPrediction + downPrediction) * 100), 2);
            downPercentage = 100 - upPercentage;
        }
    }
}