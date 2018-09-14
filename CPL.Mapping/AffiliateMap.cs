using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Mapping
{
    public class AffiliateMap : IEntityTypeConfiguration<Affiliate>
    {
        public void Configure(EntityTypeBuilder<Affiliate> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("Affiliate");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Tier1DirectRate).HasColumnName("Tier1DirectRate");
            builder.Property(t => t.Tier2SaleToTier1Rate).HasColumnName("Tier2SaleToTier1Rate");
            builder.Property(t => t.Tier3SaleToTier1Rate).HasColumnName("Tier3SaleToTier1Rate");
            builder.Property(t => t.IsAutoPaymentEnable).HasColumnName("IsAutoPaymentEnable");
            builder.Property(t => t.IsTier2TabVisible).HasColumnName("IsTier2TabVisible");
            builder.Property(t => t.IsTier3TabVisible).HasColumnName("IsTier3TabVisible");

            builder.HasOne(x => x.SysUser)
                .WithOne(x => x.Affiliate)
                .IsRequired(false)
                .HasForeignKey<SysUser>(x => x.AffiliateId);
        }
    }
}
