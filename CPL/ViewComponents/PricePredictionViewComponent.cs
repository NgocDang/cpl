using AutoMapper;
using CPL.Common.Enums;
using CPL.Core.Interfaces;
using CPL.Misc;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Mvc;
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
            var btcPriceInLocals = _btcPriceService.Queryable().Where(x => x.Time >= ((DateTimeOffset)DateTime.UtcNow.AddHours(-CPLConstant.HourBeforeInChart)).ToUnixTimeSeconds())
                .GroupBy(x => x.Time)
                .Select(y => new PricePredictionHighChartViewModel
                {
                    Time = y.Key,
                    Price = y.Select(x => x.Price).OrderByDescending(x => x).FirstOrDefault()
                })
                .ToList();

            var currentTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
            var listCurrentTime = new Dictionary<long, decimal>();
            var second = CPLConstant.HourBeforeInChart * 60 * 60 - 1; // currently 43200
            for (int j = -second; j <= 0; j++)
            {
                listCurrentTime.Add(currentTime + j, 0); // Default Price is 0;
            }

            // Join 2 list
            var pricePredictionViewModels = (from left in listCurrentTime.Keys
                                             join right in btcPriceInLocals on left equals right.Time into leftRight
                                             from lr in leftRight.DefaultIfEmpty()
                                             select new PricePredictionHighChartViewModel
                                             {
                                                 Time = left,
                                                 Price = lr?.Price,
                                             })
                                            .ToList();

            decimal value = 0;
            for (int j = 0; j < pricePredictionViewModels.Count; j++)
            {
                if (pricePredictionViewModels[j].Price != null)
                {
                    value = pricePredictionViewModels[j].Price.GetValueOrDefault(0);
                }

                pricePredictionViewModels[j].Price = value;
            }

            var previousTime = pricePredictionViewModels.FirstOrDefault().Time.ToString();
            var previousRate = string.Join(",", pricePredictionViewModels.Select(x => x.Price));
            var lowestRate = pricePredictionViewModels.Where(x => x.Price != 0).Min(x => x.Price).GetValueOrDefault(0) - CPLConstant.LowestRateBTCInterval;
            if (lowestRate < 0)
                lowestRate = 0;
            var previousBtcRate = $"{previousTime};{previousRate}";

            viewModel.PreviousBtcRate = previousBtcRate;
            viewModel.LowestBtcRate = lowestRate;
            return View(viewModel);
        }
    }
}
