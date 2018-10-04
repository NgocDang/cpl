using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Mapping
{
    public class LotteryDetailMap : IEntityTypeConfiguration<LotteryDetail>
    {
        public void Configure(EntityTypeBuilder<LotteryDetail> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("LotteryDetail");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.LotteryId).HasColumnName("LotteryId");
            builder.Property(t => t.LangId).HasColumnName("LangId");
            builder.Property(t => t.DesktopTopImage).HasColumnName("DesktopTopImage");
            builder.Property(t => t.DesktopListingImage).HasColumnName("DesktopListingImage");
            builder.Property(t => t.MobileListingImage).HasColumnName("MobileListingImage");
            builder.Property(t => t.MobileTopImage).HasColumnName("MobileTopImage");
            builder.Property(t => t.PrizeImage).HasColumnName("PrizeImage");
            builder.Property(t => t.Description).HasColumnName("Description");
            builder.Property(t => t.ShortDescription).HasColumnName("ShortDescription");

            // Relationship
            builder.HasOne(x => x.Lottery)
                .WithMany(x => x.LotteryDetails)
                .HasForeignKey(x => x.LotteryId);

            builder.HasOne(x => x.Lang)
                .WithMany(x => x.LotteryDetails)
                .HasForeignKey(x => x.LangId);
        }
    }
}
