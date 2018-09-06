using CPL.Common.Enums;
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
    class PricePredictionGetBTCPriceJob : IJob
    {
        private static int CompareIntervalInMinutes;
        public string FileName { get; set; }

        public PricePredictionGetBTCPriceJob()
        {
            FileName = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "log.txt");
        }

        public Task Execute(IJobExecutionContext context)
        {
            int pricePredictionId = DoGetBTCPrizePricePrediction(out string result);
            DoUpdateWinner(pricePredictionId, result);
            return Task.FromResult(0);
        }

        private int DoGetBTCPrizePricePrediction(out bool result)
        {
            try
            {
                var resolver = new Resolver();

                // interval time
                CompareIntervalInMinutes = int.Parse(resolver.SettingService.Queryable().FirstOrDefault(x => x.Name == PredictionGameServiceConstant.CompareIntervalInMinutes).Value);
                // currentTime
                var resultTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
                // the time to be compared
                var toBeComparedTime = ((DateTimeOffset)DateTime.UtcNow.AddMinutes(-/*CompareIntervalInMinutes*/1)).ToUnixTimeSeconds(); // TO DO
                // get price at current time from BTCPrice table
                var resultPrize = resolver.BTCPriceService.Queryable().OrderByDescending(x => x.Time).FirstOrDefault(x => resultTime >= x.Time).Price;
                // get price at time to be compared from BTCPrice table
                var toBeComparedPrize = resolver.BTCPriceService.Queryable().OrderByDescending(x => x.Time).FirstOrDefault(x => toBeComparedTime >= x.Time).Price;

                //var resultDBTime = resolver.BTCPriceService.Queryable().OrderByDescending(x => x.Time).FirstOrDefault(x => resultTime >= x.Time).Time;
                // get price at time to be compared from BTCPrice table
                //var toBeComparedDBTime = resolver.BTCPriceService.Queryable().OrderByDescending(x => x.Time).FirstOrDefault(x => toBeComparedTime >= x.Time).Time;

                // update price prediction
                var pricePrediction = resolver.PricePredictionService.Queryable().OrderBy(x => x.ResultTime).FirstOrDefault(x => !x.ResultPrice.HasValue && !x.ToBeComparedPrice.HasValue && DateTime.Now >= x.ResultTime); // TEST
                pricePrediction.ResultPrice = resultPrize;
                pricePrediction.ToBeComparedPrice = toBeComparedPrize;
                pricePrediction.UpdatedDate = DateTime.Now;

                resolver.PricePredictionService.Update(pricePrediction);

                if (resultPrize > toBeComparedPrize)
                {
                    result = EnumPricePredictionStatus.UP.ToBoolean();
                }
                else if (resultPrize < toBeComparedPrize)
                {
                    result = EnumPricePredictionStatus.DOWN.ToString();
                }
                else
                {
                    result = null; // TODO
                }

                // save to DB
                resolver.UnitOfWork.SaveChanges();

                return pricePrediction.Id;
            }
            catch (Exception ex)
            {
                if (ex.InnerException?.Message != null)
                    Utils.FileAppendThreadSafe(FileName, string.Format("DoGetBTCPrizePricePrediction -- Exception {0} at {1}{2}", ex.InnerException.Message, DateTime.Now, Environment.NewLine));
                else
                    Utils.FileAppendThreadSafe(FileName, string.Format("DoGetBTCPrizePricePrediction -- Exception {0} at {1}{2}", ex.Message, DateTime.Now, Environment.NewLine));

                result = null;
                return 0 ;
            }
        }

        private void DoUpdateWinner(int pricePredictionId, string result)
        {
            try
            {
                var resolver = new Resolver();

                // update price prediction
                var pricePredictionHistories = resolver.PricePredictionHistoryService
                    .Query()
                    .Include(x => x.SysUser)
                    .Select()
                    .Where(x => x.Id == pricePredictionId);
                foreach (var pricePredictionHistory in pricePredictionHistories)
                {
                    if (result == pricePredictionHistory.Prediction)
                    {
                        pricePredictionHistory.Result = EnumGameResult.WIN.ToString(); // TODO KYC ?
                    }
                    else // result == EnumPricePredictionStatus.LOSE.ToString()
                    {
                        pricePredictionHistory.Result = EnumGameResult.LOSE.ToString();
                    }
                }
                pricePrediction.UpdatedDate = DateTime.Now;

                resolver.PricePredictionService.Update(pricePrediction);

                // save to DB
                resolver.UnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException?.Message != null)
                    Utils.FileAppendThreadSafe(FileName, string.Format("DoGetBTCPrizePricePrediction -- Exception {0} at {1}{2}", ex.InnerException.Message, DateTime.Now, Environment.NewLine));
                else
                    Utils.FileAppendThreadSafe(FileName, string.Format("DoGetBTCPrizePricePrediction -- Exception {0} at {1}{2}", ex.Message, DateTime.Now, Environment.NewLine));
            }
        }
    }
}
