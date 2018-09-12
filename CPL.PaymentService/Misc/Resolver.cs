using Autofac;
using CPL.Core.Interfaces;
using CPL.Core.Services;
using CPL.Domain;
using CPL.Infrastructure;
using CPL.Infrastructure.Interfaces;
using CPL.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CPL.PaymentService.Misc
{
    public class Resolver
    {
        public IContainer Container { get; }
        public IUnitOfWorkAsync UnitOfWork { get; }
        public ISysUserService SysUserService { get; }
        public ILotteryService LotteryService { get; }
        public ILotteryHistoryService LotteryHistoryService { get; }
        public IPricePredictionService PricePredictionService { get; }
        public IPricePredictionHistoryService PricePredictionHistoryService { get; }
        public IPaymentService PaymentService { get; }
        public ISettingService SettingService { get; }

        public Resolver()
        {
            var builder = new ContainerBuilder();

            builder.Register(x =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<CPLContext>();
                optionsBuilder.UseSqlServer(CPLPaymentService.Configuration["ConnectionString"]);
                return optionsBuilder.Options;
            });

            builder.RegisterType<SysUserService>().As<ISysUserService>().InstancePerDependency();
            builder.RegisterType<LotteryService>().As<ILotteryService>().InstancePerDependency();
            builder.RegisterType<LotteryHistoryService>().As<ILotteryHistoryService>().InstancePerDependency();
            builder.RegisterType<PricePredictionService>().As<IPricePredictionService>().InstancePerDependency();
            builder.RegisterType<PricePredictionHistoryService>().As<IPricePredictionHistoryService>().InstancePerDependency();
            builder.RegisterType<SettingService>().As<ISettingService>().InstancePerDependency();
            builder.RegisterType<Core.Services.PaymentService>().As<IPaymentService>().InstancePerDependency();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWorkAsync>().InstancePerLifetimeScope();
            builder.RegisterType<CPLContext>().As<IDataContextAsync>().InstancePerLifetimeScope();

            builder.RegisterType<Repository<SysUser>>().As<IRepositoryAsync<SysUser>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<Lottery>>().As<IRepositoryAsync<Lottery>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<LotteryHistory>>().As<IRepositoryAsync<LotteryHistory>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<PricePrediction>>().As<IRepositoryAsync<PricePrediction>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<PricePredictionHistory>>().As<IRepositoryAsync<PricePredictionHistory>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<Payment>>().As<IRepositoryAsync<Payment>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<Setting>>().As<IRepositoryAsync<Setting>>().InstancePerLifetimeScope();

            this.Container = builder.Build();
            this.UnitOfWork = this.Container.Resolve<IUnitOfWorkAsync>();
            this.SysUserService = this.Container.Resolve<ISysUserService>();
            this.LotteryService = this.Container.Resolve<ILotteryService>();
            this.LotteryHistoryService = this.Container.Resolve<ILotteryHistoryService>();
            this.PricePredictionService = this.Container.Resolve<IPricePredictionService>();
            this.PricePredictionHistoryService = this.Container.Resolve<IPricePredictionHistoryService>();
            this.PaymentService = this.Container.Resolve<IPaymentService>();
            this.SettingService = this.Container.Resolve<ISettingService>();
        }
    }
}
