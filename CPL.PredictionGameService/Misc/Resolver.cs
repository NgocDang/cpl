using Autofac;
using CPL.Core.Interfaces;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.PredictionGameService.Misc
{
    public class Resolver
    {
        public static IContainer Container { get; set; }
        public static IUnitOfWorkAsync UnitOfWork { get; set; }
        public static IBTCPriceService BTCPriceService { get; set; }
        public static ISettingService SettingService { get; set; }
    }
}
