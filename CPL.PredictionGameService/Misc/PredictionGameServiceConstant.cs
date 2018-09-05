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

        public const int NumberOfDailyPricePrediction = 3;
        public const int PricePredictionBettingIntervalInHour = 8;
        public const int HoldingIntervalInHour = 1;
        public const int CompareIntervalInMinutes = 15;
        public static readonly CronScheduleBuilder DailyStartTime = CronScheduleBuilder.DailyAtHourAndMinute(0, 0);
    }
}
