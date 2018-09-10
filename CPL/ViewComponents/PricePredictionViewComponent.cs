using AutoMapper;
using CPL.Common.Enums;
using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Misc;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.ViewComponents
{
    public class PricePredictionViewComponent : ViewComponent
    {
        private readonly IPricePredictionService _pricePredictionService;
        private readonly IPricePredictionHistoryService _pricePredictionHistoryService;
        private readonly IBTCPriceService _btcPriceService;
        private readonly ISysUserService _sysUserService;

        public PricePredictionViewComponent(ISysUserService sysUserService,
            IBTCPriceService btcPriceService,
            IPricePredictionService pricePredictionService,
            IPricePredictionHistoryService pricePredictionHistoryService)
        {
            this._sysUserService = sysUserService;
            this._btcPriceService = btcPriceService;
            this._pricePredictionHistoryService = pricePredictionHistoryService;
            this._pricePredictionService = pricePredictionService;
        }

        public IViewComponentResult Invoke(PricePredictionViewComponentViewModel viewModel)
        {
            var tokenAmount = viewModel.TokenAmount;
            viewModel = _pricePredictionService.Queryable().Where(x => x.Id == viewModel.Id)
                .Select(x => Mapper.Map<PricePredictionViewComponentViewModel>(x)).FirstOrDefault();
            viewModel.TokenAmount = tokenAmount;

            //Calculate percentage
            decimal upPrediction = _pricePredictionHistoryService
                .Queryable()
                .Where(x => x.PricePredictionId == viewModel.Id && x.Prediction == EnumPricePredictionStatus.UP.ToBoolean())
                .Count();

            decimal downPrediction = _pricePredictionHistoryService
                .Queryable()
                .Where(x => x.PricePredictionId == viewModel.Id && x.Prediction == EnumPricePredictionStatus.DOWN.ToBoolean())
                .Count();


            if (upPrediction + downPrediction == 0)
            {
                viewModel.UpPercentage = viewModel.DownPercentage = 50;
            }
            else
            {
                viewModel.UpPercentage = Math.Round((upPrediction / (upPrediction + downPrediction) * 100), 2);
                viewModel.DownPercentage = 100 - viewModel.UpPercentage;
            }
            //////////////////////////

            var btcCurrentPriceResult = ServiceClient.BTCCurrentPriceClient.GetBTCCurrentPriceAsync();
            btcCurrentPriceResult.Wait();
            if (btcCurrentPriceResult.Result.Status.Code == 0)
            {
                viewModel.CurrentBTCRate = btcCurrentPriceResult.Result.Price;
                viewModel.CurrentBTCRateInString = btcCurrentPriceResult.Result.Price.ToString("#,##0.00");
            }

            // Get btc previous rates 12h before until now
            var btcPriceInUTC = _btcPriceService.Queryable()
                .Where(x => x.Time >= ((DateTimeOffset)viewModel.OpenBettingTime.AddHours(-CPLConstant.HourBeforeInChart)).ToUnixTimeSeconds())
                .ToList();
            var lowestRate = btcPriceInUTC.Min(x => x.Price) - CPLConstant.LowestRateBTCInterval;
            if (lowestRate < 0)
                lowestRate = 0;
            viewModel.PreviousBtcRate = JsonConvert.SerializeObject(btcPriceInUTC);
            viewModel.LowestBtcRate = lowestRate;
            return View(viewModel);
        }
    }
}
