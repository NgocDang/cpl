using Autofac;
using CPL.Core.Interfaces;
using CPL.Core.Services;
using CPL.Domain;
using CPL.Infrastructure;
using CPL.Infrastructure.Interfaces;
using CPL.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.PredictionGameService.Misc
{
    public class Resolver
    {
        public IContainer Container { get; }
        public IUnitOfWorkAsync UnitOfWork { get; }
        public ISysUserService SysUserService { get; }
        public IPricePredictionService PricePredictionService { get; }
        public IPricePredictionHistoryService PricePredictionHistoryService { get; }
        public ISettingService SettingService { get; }
        public IBTCPriceService BTCPriceService { get; }

        public Resolver()
        {
            var builder = new ContainerBuilder();

            builder.Register(x =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<CPLContext>();
                optionsBuilder.UseSqlServer(CPLPredictionGameService.Configuration["ConnectionString"]);
                return optionsBuilder.Options;
            });

            builder.RegisterType<SysUserService>().As<ISysUserService>().InstancePerDependency();
            builder.RegisterType<PricePredictionService>().As<IPricePredictionService>().InstancePerDependency();
            builder.RegisterType<SettingService>().As<ISettingService>().InstancePerDependency();
            builder.RegisterType<BTCPriceService>().As<IBTCPriceService>().InstancePerDependency();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWorkAsync>().InstancePerLifetimeScope();
            builder.RegisterType<CPLContext>().As<IDataContextAsync>().InstancePerLifetimeScope();

            builder.RegisterType<Repository<SysUser>>().As<IRepositoryAsync<SysUser>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<PricePrediction>>().As<IRepositoryAsync<PricePrediction>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<PricePredictionHistory>>().As<IRepositoryAsync<PricePredictionHistory>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<Setting>>().As<IRepositoryAsync<Setting>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<BTCPrice>>().As<IRepositoryAsync<BTCPrice>>().InstancePerLifetimeScope();

            this.Container = builder.Build();
            this.UnitOfWork = this.Container.Resolve<IUnitOfWorkAsync>();
            this.SysUserService = this.Container.Resolve<ISysUserService>();
            this.PricePredictionService = this.Container.Resolve<IPricePredictionService>();
            this.PricePredictionHistoryService = this.Container.Resolve<IPricePredictionHistoryService>();
            this.SettingService = this.Container.Resolve<ISettingService>();
            this.BTCPriceService = this.Container.Resolve<IBTCPriceService>();
        }
    }
}
