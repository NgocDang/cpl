using CPL.LotteryGameService.Misc;
using Microsoft.Extensions.PlatformAbstractions;
using PeterKottas.DotNetCore.WindowsService;
using System;
using System.IO;

namespace CPL.LotteryGameService
{
    class Program
    {
        public static void Main(string[] args)
        {
            ServiceRunner<CPLLotteryGameService>.Run(config =>
            {
                string fileName = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "log.txt");

                config.SetDisplayName(WSConstant.ServiceName);
                config.SetName(WSConstant.ServiceName);
                config.SetDescription(WSConstant.ServiceDescription);

                config.Service(serviceConfig =>
                {
                    serviceConfig.ServiceFactory((extraArguments, controller) =>
                    {
                        return new CPLLotteryGameService();
                    });

                    serviceConfig.OnStart((service, extraParams) =>
                    {
                        service.Start();
                    });

                    serviceConfig.OnStop(service =>
                    {
                        service.Stop();
                    });

                    serviceConfig.OnError(e =>
                    {
                        Utils.FileAppendThreadSafe(fileName, string.Format("Exception: {0}{1}{2}", e.StackTrace.ToString(), DateTime.Now, Environment.NewLine));
                    });
                });
            });
        }
    }
}
