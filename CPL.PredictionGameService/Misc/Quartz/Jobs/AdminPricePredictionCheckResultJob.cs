using CPL.Common.Misc;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPL.PredictionGameService.Misc.Quartz.Jobs
{
    internal class AdminPricePredictionCheckResultJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Utils.FileAppendThreadSafe(CPLPredictionGameService.basePricePredictionFunctions.FileName, string.Format("{0}Execute AdminPricePredictionCheckResultJob {1}: {2}", Environment.NewLine, DateTime.Now, Environment.NewLine));

            JobDataMap dataMap = context.JobDetail.JobDataMap;
            Resolver resolver = (Resolver)dataMap["Resolver"];
            var pricePredictionId = (int)dataMap["PricePredictionId"];
            DateTime localDateTime = context.FireTimeUtc.LocalDateTime;

            // Update result game
            var pricePrediction = resolver.PricePredictionService.Queryable().FirstOrDefault(x => x.Id == pricePredictionId);
            if (pricePrediction != null)
            {
                CPLPredictionGameService.basePricePredictionFunctions.DoGetBTCPrice(ref resolver, pricePrediction.Id);
                CPLPredictionGameService.basePricePredictionFunctions.DoUpdateWinner(ref resolver, pricePrediction.Id);
            }

            return Task.FromResult(0);
        }
    }
}
