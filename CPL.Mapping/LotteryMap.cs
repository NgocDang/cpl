﻿using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Mapping
{
    public class LotteryMap : IEntityTypeConfiguration<Lottery>
    {
        public void Configure(EntityTypeBuilder<Lottery> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("Lottery");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Phase).HasColumnName("Phase");
            builder.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            builder.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(t => t.Volume).HasColumnName("Volume");
            builder.Property(t => t.Status).HasColumnName("Status");
            builder.Property(t => t.DesktopTopImage).HasColumnName("DesktopTopImage");
            builder.Property(t => t.MobileTopImage).HasColumnName("MobileTopImage");
            builder.Property(t => t.DesktopListingImage).HasColumnName("DesktopListingImage");
            builder.Property(t => t.MobileListingImage).HasColumnName("MobileListingImage");
            builder.Property(t => t.PrizeImage).HasColumnName("PrizeImage");
            builder.Property(t => t.Title).HasColumnName("Title");
            builder.Property(t => t.UnitPrice).HasColumnName("UnitPrice");

            //Relationship
            builder.HasOne(x => x.LotteryCategory)
                .WithMany(x => x.Lotteries)
                .HasForeignKey(x => x.LotteryCategoryId);
        }
    }
}
