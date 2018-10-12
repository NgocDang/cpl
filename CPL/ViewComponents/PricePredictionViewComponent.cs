using AutoMapper;
using CPL.Common.Enums;
using CPL.Common.Misc;
using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Misc;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

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
            var predictedTrend = viewModel.PredictedTrend;
            var isDisabled = viewModel.IsDisabled;
            var coinBase = viewModel.Coinbase;

            viewModel = _pricePredictionService.Queryable().Where(x => x.Id == viewModel.Id)
                .Select(x => Mapper.Map<PricePredictionViewComponentViewModel>(x)).FirstOrDefault();
            viewModel.TokenAmount = tokenAmount;
            viewModel.PredictedTrend = predictedTrend;
            viewModel.IsDisabled = isDisabled;
            viewModel.BTCPricePredictionChartTitle = ((EnumCurrencyPair)Enum.Parse(typeof(EnumCurrencyPair), coinBase)) == EnumCurrencyPair.BTCUSDT ? LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "BTCPricePredictionChartTitle") : ""; // TODO: Add more chart title if there are more coinbases
            viewModel.BTCPricePredictionSeriesName = ((EnumCurrencyPair)Enum.Parse(typeof(EnumCurrencyPair), coinBase)) == EnumCurrencyPair.BTCUSDT ? LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "BTCPricePredictionSeriesName") : ""; // TODO: Add more chart title if there are more coinbases

            //Calculate percentage
            decimal highPrediction = _pricePredictionHistoryService
                .Queryable()
                .Where(x => x.PricePredictionId == viewModel.Id && x.Prediction == EnumPricePredictionStatus.HIGH.ToBoolean() && x.Result != EnumGameResult.REFUND.ToString())
                .Count();

            decimal lowPrediction = _pricePredictionHistoryService
                .Queryable()
                .Where(x => x.PricePredictionId == viewModel.Id && x.Prediction == EnumPricePredictionStatus.LOW.ToBoolean() && x.Result != EnumGameResult.REFUND.ToString())
                .Count();


            if (highPrediction + lowPrediction == 0)
            {
                viewModel.HighPercentage = viewModel.LowPercentage = 50;
            }
            else
            {
                viewModel.HighPercentage = Math.Round((highPrediction / (highPrediction + lowPrediction) * 100), 2);
                viewModel.LowPercentage = 100 - viewModel.HighPercentage;
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
                .Where(x => x.Time >= viewModel.OpenBettingTime.AddHours(-CPLConstant.HourBeforeInChart).ToUTCUnixTimeInSeconds())
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
