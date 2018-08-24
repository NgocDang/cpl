using CPL.Common.Misc;
using CPL.NotifyService.Misc;
using Microsoft.Extensions.PlatformAbstractions;
using PeterKottas.DotNetCore.WindowsService;
using System;
using System.IO;

namespace CPL.NotifyService
{
    class Program
    {
        public static void Main(string[] args)
        {
            ServiceRunner<CPLNotifyService>.Run(config =>
            {
                string fileName = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "log.txt");

                config.SetDisplayName(NotifyServiceConstant.ServiceName);
                config.SetName(NotifyServiceConstant.ServiceName);
                config.SetDescription(NotifyServiceConstant.ServiceDescription);

                config.Service(serviceConfig =>
                {
                    serviceConfig.ServiceFactory((extraArguments, controller) =>
                    {
                        return new CPLNotifyService();
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
