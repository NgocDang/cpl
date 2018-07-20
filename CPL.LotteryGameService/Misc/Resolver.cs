using Autofac;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.LotteryGameService.Misc
{
    public class Resolver
    {
        public static IContainer Container { get; set; }
        public static IUnitOfWorkAsync UnitOfWork { get; set; }
    }
}
