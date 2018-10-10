using CPL.Common.Misc;
using Microsoft.Extensions.PlatformAbstractions;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPL.PredictionGameService.Misc.Quartz.Jobs
{
    internal class AdminPricePredictionCheckResultJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            Resolver resolver = (Resolver)dataMap["Resolver"];
            var pricePredictionId = (int)dataMap["PricePredictionId"];
            BasePricePredictionFunctions AdminBasePricePredictionFunctions = (BasePricePredictionFunctions)dataMap["AdminBasePricePredictionFunctions"];
            string fileName = (string)dataMap["AdminLogFileName"];
            DateTime localDateTime = context.FireTimeUtc.LocalDateTime;

            Utils.FileAppendThreadSafe(fileName, string.Format("{0}Execute AdminPricePredictionCheckResultJob {1}: {2}", Environment.NewLine, DateTime.Now, Environment.NewLine));

            // Update result game
            var pricePrediction = resolver.PricePredictionService.Queryable().FirstOrDefault(x => x.Id == pricePredictionId);
            if (pricePrediction != null)
            {
                AdminBasePricePredictionFunctions.DoGetBTCPrice(ref resolver, pricePrediction.Id, fileName);
                AdminBasePricePredictionFunctions.DoUpdateWinner(ref resolver, pricePrediction.Id, fileName);
            }

            return Task.FromResult(0);
        }
    }
}
