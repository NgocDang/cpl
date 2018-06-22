using CPL.Domain;
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
        public DbSet<Team> Team { get; set; }
        public DbSet<Notification> Notification { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SysUserMap());
            modelBuilder.ApplyConfiguration(new SettingMap());
            modelBuilder.ApplyConfiguration(new LangMap());
            modelBuilder.ApplyConfiguration(new LangDetailMap());
            modelBuilder.ApplyConfiguration(new TemplateMap());
            modelBuilder.ApplyConfiguration(new CurrencyMap());
            modelBuilder.ApplyConfiguration(new LangMsgDetailMap());
            modelBuilder.ApplyConfiguration(new TeamMap());
            modelBuilder.ApplyConfiguration(new NotificationMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
