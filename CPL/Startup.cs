﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CPL.Common.Enums;
using CPL.Core.Interfaces;
using CPL.Core.Services;
using CPL.Domain;
using CPL.Infrastructure;
using CPL.Infrastructure.Interfaces;
using CPL.Infrastructure.Repositories;
using CPL.Misc;
using CPL.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                .AddScoped<IRepositoryAsync<LangDetail>, Repository<LangDetail>>()
                .AddScoped<IRepositoryAsync<Lang>, Repository<Lang>>()
                .AddScoped<IRepositoryAsync<Setting>, Repository<Setting>>()
                .AddScoped<IRepositoryAsync<SysUser>, Repository<SysUser>>()
                .AddScoped<IRepositoryAsync<Template>, Repository<Template>>()
                .AddScoped<IRepositoryAsync<Currency>, Repository<Currency>>()
                .AddScoped<IRepositoryAsync<Notification>, Repository<Notification>>()
                .AddScoped<IRepositoryAsync<LangMsgDetail>, Repository<LangMsgDetail>>()
                .AddScoped<IRepositoryAsync<Team>, Repository<Team>>()

                .AddScoped<IUnitOfWorkAsync, UnitOfWork>()
                .AddScoped<IDataContextAsync, CPLContext>();

            services.AddDistributedMemoryCache();
            services.AddAutoMapper();
            services.AddMvc().AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddSession();

            services
                .AddTransient<ILangService, LangService>()
                .AddTransient<ILangDetailService, LangDetailService>()
                .AddTransient<ISysUserService, SysUserService>()
                .AddTransient<ISettingService, SettingService>()
                .AddTransient<ITemplateService, TemplateService>()
                .AddTransient<ICurrencyService, CurrencyService>()
                .AddTransient<ILangMsgDetailService, LangMsgDetailService>()
                .AddTransient<ITeamService, TeamService>()
                .AddTransient<INotificationService, NotificationService>()
                .AddTransient<IViewRenderService, ViewRenderService>()
                .AddTransient<ICoinTransactionService, CoinTransactionService>()
                .AddTransient<IGameService, GameService>()
                .AddTransient<IGameHistoryService, GameHistoryService>()
                .AddTransient<IRateService, RateService>();
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
            LoadSetting(serviceProvider);
            LoadWCF();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseMvcWithDefaultRoute();
        }

        private void LoadWCF()
        {

        }

        private void LoadLangDetail(IServiceProvider serviceProvider)
        {
            LangDetailHelper.LangDetails = ((LangDetailService)serviceProvider.GetService(typeof(ILangDetailService))).Queryable().Select(x => Mapper.Map<LangDetailViewModel>(x)).ToList();
        }

        private void LoadSetting(IServiceProvider serviceProvider)
        {
            CPLConstant.Maintenance.IsOnMaintenance = bool.Parse(((SettingService)serviceProvider.GetService(typeof(ISettingService))).Queryable().FirstOrDefault(x => x.Name == CPLConstant.Maintenance.IsOnMaintenanceSetting).Value);
        }
    }
}