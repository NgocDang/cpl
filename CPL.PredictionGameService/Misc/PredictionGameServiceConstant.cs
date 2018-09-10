using Quartz;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.PredictionGameService.Misc
{
    public class PredictionGameServiceConstant
    {
        public const string ServiceName = "CPL Prediction Game Service";
        public const string ServiceDescription = "The service to work for prediction game in CPL project";

        public const string NumberOfDailyPricePrediction = "NumberOfDailyPricePrediction";
        public const string PricePredictionBettingIntervalInHour = "PricePredictionBettingIntervalInHour";
        public const string HoldingIntervalInHour = "HoldingIntervalInHour";
        public const string CompareIntervalInMinute = "CompareIntervalInMinutes";
        public const string DailyStartTimeInHour = "DailyStartTimeInHour";
        public const string DailyStartTimeInMinute = "DailyStartTimeInMinute";
    }
}
