using System;
using System.Linq;
using System.ServiceModel;
using AutoMapper;
using CPL.Common.Enums;
using CPL.Core.Interfaces;
using CPL.Core.Services;
using CPL.Domain;
using CPL.Hubs;
using CPL.Infrastructure;
using CPL.Infrastructure.Interfaces;
using CPL.Infrastructure.Repositories;
using CPL.Misc;
using CPL.Misc.Quartz;
using CPL.Misc.Quartz.Factories;
using CPL.Misc.Quartz.Interfaces;
using CPL.Misc.Quartz.Jobs;
using CPL.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace CPL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CPLContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:CPLConnection"]));

            services
                .AddScoped<IRepositoryAsync<MobileLangDetail>, Repository<MobileLangDetail>>()
                .AddScoped<IRepositoryAsync<MobileLangMsgDetail>, Repository<MobileLangMsgDetail>>()
                .AddScoped<IRepositoryAsync<LangDetail>, Repository<LangDetail>>()
                .AddScoped<IRepositoryAsync<Lang>, Repository<Lang>>()
                .AddScoped<IRepositoryAsync<LangContent>, Repository<LangContent>>()
                .AddScoped<IRepositoryAsync<Setting>, Repository<Setting>>()
                .AddScoped<IRepositoryAsync<SysUser>, Repository<SysUser>>()
                .AddScoped<IRepositoryAsync<Template>, Repository<Template>>()
                .AddScoped<IRepositoryAsync<Currency>, Repository<Currency>>()
                .AddScoped<IRepositoryAsync<Notification>, Repository<Notification>>()
                .AddScoped<IRepositoryAsync<LangMsgDetail>, Repository<LangMsgDetail>>()
                .AddScoped<IRepositoryAsync<Team>, Repository<Team>>()
                .AddScoped<IRepositoryAsync<CoinTransaction>, Repository<CoinTransaction>>()
                .AddScoped<IRepositoryAsync<PricePrediction>, Repository<PricePrediction>>()
                .AddScoped<IRepositoryAsync<PricePredictionHistory>, Repository<PricePredictionHistory>>()
                .AddScoped<IRepositoryAsync<Lottery>, Repository<Lottery>>()
                .AddScoped<IRepositoryAsync<LotteryHistory>, Repository<LotteryHistory>>()
                .AddScoped<IRepositoryAsync<LotteryPrize>, Repository<LotteryPrize>>()
                .AddScoped<IRepositoryAsync<BTCPrice>, Repository<BTCPrice>>()
                .AddScoped<IRepositoryAsync<News>, Repository<News>>()
                .AddScoped<IRepositoryAsync<Contact>, Repository<Contact>>()
                .AddScoped<IRepositoryAsync<BTCTransaction>, Repository<BTCTransaction>>()
                .AddScoped<IRepositoryAsync<ETHTransaction>, Repository<ETHTransaction>>()
                .AddScoped<IRepositoryAsync<Agency>, Repository<Agency>>()
                .AddScoped<IRepositoryAsync<AgencyToken>, Repository<AgencyToken>>()
                .AddScoped<IRepositoryAsync<Affiliate>, Repository<Affiliate>>()
                .AddScoped<IUnitOfWorkAsync, UnitOfWork>()
                .AddScoped<IDataContextAsync, CPLContext>();

            services.AddDistributedMemoryCache();
            services.AddAutoMapper();
            services.AddMvc().AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddSession();

            services.AddSingleton<ILotteryDrawingFactory, LotteryDrawingFactory>();

            services.UseQuartz<ILotteryDrawingFactory>(typeof(LotteryDrawingJob));

            services
                .AddTransient<ILangService, LangService>()
                .AddTransient<ILangDetailService, LangDetailService>()
                .AddTransient<IMobileLangDetailService, MobileLangDetailService>()
                .AddTransient<IMobileLangMsgDetailService, MobileLangMsgDetailService>()
                .AddTransient<ILangContentService, LangContentService>()
                .AddTransient<ISysUserService, SysUserService>()
                .AddTransient<ISettingService, SettingService>()
                .AddTransient<ITemplateService, TemplateService>()
                .AddTransient<ICurrencyService, CurrencyService>()
                .AddTransient<ILangMsgDetailService, LangMsgDetailService>()
                .AddTransient<ITeamService, TeamService>()
                .AddTransient<INotificationService, NotificationService>()
                .AddTransient<IViewRenderService, ViewRenderService>()
                .AddTransient<ICoinTransactionService, CoinTransactionService>()
                .AddTransient<IPricePredictionService, PricePredictionService>()
                .AddTransient<IPricePredictionHistoryService, PricePredictionHistoryService>()
                .AddTransient<ILotteryService, LotteryService>()
                .AddTransient<ILotteryHistoryService, LotteryHistoryService>()
                .AddTransient<ILotteryPrizeService, LotteryPrizeService>()
                .AddTransient<IQuartzSchedulerService, QuartzSchedulerService>()
                .AddTransient<IBTCPriceService, BTCPriceService>()
                .AddTransient<INewsService, NewsService>()
                .AddTransient<IBTCTransactionService, BTCTransactionService>()
                .AddTransient<IETHTransactionService, ETHTransactionService>()
                .AddTransient<IAgencyService, AgencyService>()
                .AddTransient<IAgencyTokenService, AgencyTokenService>()
                .AddTransient<IAffiliateService, AffiliateService>()
                .AddTransient<INewsService, NewsService>()
                .AddTransient<IContactService, ContactService>();

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();

            LoadLangDetail(serviceProvider);
            LoadLangMsgDetail(serviceProvider);
            LoadSetting(serviceProvider);
            LoadWCF(serviceProvider);
            LoadQuartz(serviceProvider);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<UserPredictionProgressHub>("/predictedUserProgress");
            });
            app.UseMvcWithDefaultRoute();
        }

        private void LoadWCF(IServiceProvider serviceProvider)
        {
            // Load URL Endpoint
            var fhCoreServiceEndpoint = ((SettingService)serviceProvider.GetService(typeof(ISettingService))).Queryable().FirstOrDefault(x => x.Name == CPLConstant.FHCoreServiceEndpoint).Value;

            // Set endpoint address
            // Authentication Service
            ServiceClient.AuthenticationClient = new AuthenticationService.AuthenticationClient();
            ServiceClient.AuthenticationClient.Endpoint.Address = new EndpointAddress(new Uri(fhCoreServiceEndpoint + CPLConstant.AuthenticationServiceEndpoint));

            // Email Service
            ServiceClient.EmailClient = new EmailService.EmailClient();
            ServiceClient.EmailClient.Endpoint.Address = new EndpointAddress(new Uri(fhCoreServiceEndpoint + CPLConstant.EmailServiceEndpoint));

            // BTC Current price service
            var cplServiceEndpoint = ((SettingService)serviceProvider.GetService(typeof(ISettingService))).Queryable().FirstOrDefault(x => x.Name == CPLConstant.CPLServiceEndpoint).Value;

            ServiceClient.BTCCurrentPriceClient = new BTCCurrentPriceService.BTCCurrentPriceClient();
            ServiceClient.BTCCurrentPriceClient.Endpoint.Address = new EndpointAddress(new Uri(cplServiceEndpoint + CPLConstant.BTCCurrentPriceServiceEndpoint));

            // EToken service
            ServiceClient.ETokenClient = new ETokenService.ETokenClient();
            ServiceClient.ETokenClient.Endpoint.Address = new EndpointAddress(new Uri(fhCoreServiceEndpoint + CPLConstant.ETokenServiceEndpoint));

            // EWallet Service
            ServiceClient.EWalletClient = new EWalletService.EWalletClient();
            ServiceClient.EWalletClient.Endpoint.Address = new EndpointAddress(new Uri(fhCoreServiceEndpoint + CPLConstant.EWalletServiceEndpoint));

            // BWallet Service
            ServiceClient.BWalletClient = new BWalletService.BWalletClient();
            ServiceClient.BWalletClient.Endpoint.Address = new EndpointAddress(new Uri(fhCoreServiceEndpoint + CPLConstant.BWalletServiceEndpoint));

            // EAccount Service
            ServiceClient.EAccountClient = new EAccountService.EAccountClient();
            ServiceClient.EAccountClient.Endpoint.Address = new EndpointAddress(new Uri(fhCoreServiceEndpoint + CPLConstant.EAccountServiceEndpoint));

            // BAccount Service
            ServiceClient.BAccountClient = new BAccountService.BAccountClient();
            ServiceClient.BAccountClient.Endpoint.Address = new EndpointAddress(new Uri(fhCoreServiceEndpoint + CPLConstant.BAccountServiceEndpoint));

            // Authentication
            var authentication = ServiceClient.AuthenticationClient.AuthenticateAsync(CPLConstant.ProjectEmail, CPLConstant.ProjectName);
            authentication.Wait();
            Authentication.Token = authentication.Result.Token;

            var eToken = ServiceClient.ETokenClient.SetAsync(Authentication.Token, new ETokenService.ETokenSetting { Abi = CPLConstant.Abi, ContractAddress = CPLConstant.SmartContractAddress, Environment = (ETokenService.Environment)((int)CPLConstant.Environment), Platform = ETokenService.Platform.ETH });
            eToken.Wait();

            var eWallet = ServiceClient.EWalletClient.SetAsync(Authentication.Token, new EWalletService.EWalletSetting { Environment = (EWalletService.Environment)((int)CPLConstant.Environment) });
            eWallet.Wait();

            var bWallet = ServiceClient.BWalletClient.SetAsync(Authentication.Token, new BWalletService.BWalletSetting { Environment = (BWalletService.Environment)((int)CPLConstant.Environment) });
            bWallet.Wait();

            var eAccount = ServiceClient.EAccountClient.SetAsync(Authentication.Token, new EAccountService.EAccountSetting { Environment = (EAccountService.Environment)((int)CPLConstant.Environment), Platform = EAccountService.Platform.ETH});
            eAccount.Wait();

            var bAccount = ServiceClient.BAccountClient.SetAsync(Authentication.Token, new BAccountService.BAccountSetting { Environment = (BAccountService.Environment)((int)CPLConstant.Environment), Platform = BAccountService.Platform.BTC});
            bAccount.Wait();
        }

        private void LoadLangDetail(IServiceProvider serviceProvider)
        {
            LangDetailHelper.LangDetails = ((LangDetailService)serviceProvider.GetService(typeof(ILangDetailService))).Queryable().Select(x => Mapper.Map<LangDetailViewModel>(x)).ToList();
        }

        private void LoadLangMsgDetail(IServiceProvider serviceProvider)
        {
            LangMsgDetailHelper.LangMsgDetails = ((LangMsgDetailService)serviceProvider.GetService(typeof(ILangMsgDetailService))).Queryable().Select(x => Mapper.Map<LangMsgDetailViewModel>(x)).ToList();
        }

        private void LoadSetting(IServiceProvider serviceProvider)
        {
            CPLConstant.Maintenance.IsOnMaintenance = bool.Parse(((SettingService)serviceProvider.GetService(typeof(ISettingService))).Queryable().FirstOrDefault(x => x.Name == CPLConstant.Maintenance.IsOnMaintenanceSetting).Value);
        }

        private void LoadQuartz(IServiceProvider serviceProvider)
        {
            // Drawing lottery job
            var scheduler = serviceProvider.GetScheduler<IScheduler, ILotteryDrawingFactory>();
            var drawingTime = DateTime.Parse(((SettingService)serviceProvider.GetService(typeof(ISettingService))).Queryable().FirstOrDefault(x => x.Name == CPLConstant.LotteryGameDrawingInHourOfDay).Value);
            QuartzHelper.StartJob<LotteryDrawingJob>(scheduler, drawingTime);
        }
    }
}
