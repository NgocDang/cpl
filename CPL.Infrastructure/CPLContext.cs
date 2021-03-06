﻿using CPL.Domain;
using CPL.Infrastructure.Repositories;
using CPL.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Infrastructure
{
    public class CPLContext : DataContext
    {
        public CPLContext(DbContextOptions<CPLContext> options)
            : base(options)
        {
        }

        public DbSet<SysUser> SysUser { get; set; }
        public DbSet<Setting> Setting { get; set; }
        public DbSet<Lang> Lang { get; set; }
        public DbSet<LangDetail> LangDetail { get; set; }
        public DbSet<Template> Template { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<LangMsgDetail> LangMsgDetail { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<CoinTransaction> CoinTransaction { get; set; }
        public DbSet<PricePredictionCategory> PricePredictionCategory { get; set; }
        public DbSet<PricePredictionCategoryDetail> PricePredictionCategoryDetail { get; set; }
        public DbSet<PricePredictionSetting> PricePredictionSetting { get; set; }
        public DbSet<PricePredictionSettingDetail> PricePredictionSettingDetail { get; set; }
        public DbSet<PricePrediction> PricePrediction { get; set; }
        public DbSet<PricePredictionDetail> PredictionDetail { get; set; }
        public DbSet<PricePredictionHistory> PricePredictionHistory { get; set; }
        public DbSet<Lottery> Lottery { get; set; }
        public DbSet<LotteryHistory> LotteryHistory { get; set; }
        public DbSet<LotteryPrize> LotteryPrize { get; set; }
        public DbSet<BTCPrice> BTCPrice { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<BTCTransaction> BTCTransaction { get; set; }
        public DbSet<ETHTransaction> ETHTransaction { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Agency> Agency { get; set; }
        public DbSet<AgencyToken> AgencyToken { get; set; }
        public DbSet<Affiliate> Affiliate { get; set; }
        public DbSet<LangContent> LangContent { get; set; }
        public DbSet<LotteryCategory> LotteryCategory { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<LotteryDetail> LotteryDetail { get; set; }
        public DbSet<IntroducedUsers> IntroducedUsers { get; set; }
        public DbSet<Group>  Groups { get; set; }
        public DbSet<Slider> Slider { get; set; }
        public DbSet<SliderDetail> SliderDetail { get; set; }
        public DbSet<FAQ> FAQ { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SysUserMap());
            modelBuilder.ApplyConfiguration(new SettingMap());
            modelBuilder.ApplyConfiguration(new LangMap());
            modelBuilder.ApplyConfiguration(new LangDetailMap());
            modelBuilder.ApplyConfiguration(new LangContentMap());
            modelBuilder.ApplyConfiguration(new TemplateMap());
            modelBuilder.ApplyConfiguration(new CurrencyMap());
            modelBuilder.ApplyConfiguration(new LangMsgDetailMap());
            modelBuilder.ApplyConfiguration(new NotificationMap());

            modelBuilder.ApplyConfiguration(new CoinTransactionMap());

            modelBuilder.ApplyConfiguration(new PricePredictionCategoryMap());
            modelBuilder.ApplyConfiguration(new PricePredictionCategoryDetailMap());
            modelBuilder.ApplyConfiguration(new PricePredictionSettingMap());
            modelBuilder.ApplyConfiguration(new PricePredictionSettingDetailMap());
            modelBuilder.ApplyConfiguration(new PricePredictionMap());
            modelBuilder.ApplyConfiguration(new PricePredictionDetailMap());
            modelBuilder.ApplyConfiguration(new PricePredictionHistoryMap());

            modelBuilder.ApplyConfiguration(new LotteryMap());
            modelBuilder.ApplyConfiguration(new LotteryPrizeMap());
            modelBuilder.ApplyConfiguration(new LotteryHistoryMap());
            modelBuilder.ApplyConfiguration(new LotteryCategoryMap());
            modelBuilder.ApplyConfiguration(new LotteryDetailMap());

            modelBuilder.ApplyConfiguration(new BTCPriceMap());

            modelBuilder.ApplyConfiguration(new NewsMap());

            modelBuilder.ApplyConfiguration(new AgencyMap());
            modelBuilder.ApplyConfiguration(new AgencyTokenMap());
            modelBuilder.ApplyConfiguration(new AffiliateMap());

            modelBuilder.ApplyConfiguration(new PaymentMap());

            modelBuilder.ApplyConfiguration(new BTCTransactionMap());
            modelBuilder.ApplyConfiguration(new ETHTransactionMap());

            modelBuilder.ApplyConfiguration(new IntroducedUsersMap());

            modelBuilder.ApplyConfiguration(new GroupMap());
            modelBuilder.ApplyConfiguration(new SliderMap());
            modelBuilder.ApplyConfiguration(new SliderDetailMap());

            modelBuilder.ApplyConfiguration(new FAQMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
