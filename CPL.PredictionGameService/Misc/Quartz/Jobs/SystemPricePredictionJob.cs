using CPL.Common.Enums;
using CPL.Common.Misc;
using CPL.Core.Services;
using CPL.Domain;
using CPL.Infrastructure.Repositories;
using Microsoft.Extensions.PlatformAbstractions;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPL.PredictionGameService.Misc.Quartz.Jobs
{
    internal class SystemPricePredictionJob : IJob
    {
        private static int PricePredictionBettingIntervalInHour;        // 8h
        private static int PricePredictionHoldingIntervalInHour;        // 1h
        private static int PricePredictionCompareIntervalInMinute;      // 15m
        private static int PricePredictionGameIntervalInHour;           // 24h

        public Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            Resolver resolver = (Resolver)dataMap["Resolver"];
            string fileName = (string)dataMap["AdminLogFileName"];
            BasePricePredictionFunctions systemBasePricePredictionFunctions = (BasePricePredictionFunctions)dataMap["SystemBasePricePredictionFunctions"];

            Utils.FileAppendThreadSafe(fileName, string.Format("{0}Execute SystemPricePredictionCreatingJob {1}: {2}", Environment.NewLine, DateTime.Now, Environment.NewLine));
                        
            PricePredictionBettingIntervalInHour = int.Parse(resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.PricePredictionBettingIntervalInHour).Value);
            PricePredictionHoldingIntervalInHour = int.Parse(resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.PricePredictionHoldingIntervalInHour).Value);
            PricePredictionCompareIntervalInMinute = int.Parse(resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.PricePredictionCompareIntervalInMinute).Value);
            PricePredictionGameIntervalInHour = int.Parse(resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.PricePredictionGameIntervalInHour).Value);

            DateTime localDateTime = context.FireTimeUtc.LocalDateTime;

            // Create new price prediction game
            DoCreatePricePrediction(ref resolver, localDateTime, fileName);

            // Update result game
            var pricePrediction = resolver.PricePredictionService.Queryable().FirstOrDefault(x => !x.ResultPrice.HasValue && !x.ToBeComparedPrice.HasValue && localDateTime.ToString("dd-MM-yyyy HH:mm") == x.ResultTime.ToString("dd-MM-yyyy HH:mm") && x.PricePredictionCategoryId == (int)EnumPricePredictionCategory.SYSTEM);
            if (pricePrediction != null)
            {
                systemBasePricePredictionFunctions.DoGetBTCPrice(ref resolver, pricePrediction.Id, fileName);
                systemBasePricePredictionFunctions.DoUpdateWinner(ref resolver, pricePrediction.Id, fileName);
            }

            return Task.FromResult(0);
        }

        public void DoCreatePricePrediction(ref Resolver resolver, DateTime localDateTime, string fileName)
        {
            Utils.FileAppendThreadSafe(fileName, string.Format("1. DoCreateSystemPricePrediction--OpenBettingTime is {0}: {1}{2}", localDateTime, DateTime.Now, Environment.NewLine));

            var newPricePredictionRecord = new PricePrediction
            {
                Coinbase = EnumCurrencyPair.BTCUSDT.ToString(),
                OpenBettingTime = localDateTime,
                CloseBettingTime = localDateTime.AddHours(PricePredictionGameIntervalInHour - PricePredictionHoldingIntervalInHour).AddMinutes(-PricePredictionCompareIntervalInMinute),
                ToBeComparedTime = localDateTime.AddHours(PricePredictionGameIntervalInHour).AddMinutes(-PricePredictionCompareIntervalInMinute),
                ResultTime = localDateTime.AddHours(PricePredictionGameIntervalInHour),
                PricePredictionCategoryId = (int)EnumPricePredictionCategory.SYSTEM,
                DividendRate = int.Parse(resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.PricePredictionTotalAwardPercentage).Value)
            };

            resolver.PricePredictionService.Insert(newPricePredictionRecord);

            // udpate DB
            resolver.UnitOfWork.SaveChanges();

            var langIds = resolver.LangService.Queryable().Select(x => x.Id).ToList();
            var title = newPricePredictionRecord.CloseBettingTime.ToString("HH:mm") == "00:00" ? "24:00" : newPricePredictionRecord.CloseBettingTime.ToString("HH:mm");
            foreach (var id in langIds)
            {
                resolver.PricePredictionDetailService.Insert(new PricePredictionDetail
                {
                    LangId = id,
                    Title = title, // title will be shown as tab name in priceprediction screen
                    PricePredictionId = newPricePredictionRecord.Id,
                });
            }

            // udpate DB
            resolver.UnitOfWork.SaveChanges();
        }
    }
}
