using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Mapping
{
    public class PaymentMap : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("Payment");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.SysUserId).HasColumnName("SysUserId");
            builder.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            builder.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(t => t.Tier1DirectSale).HasColumnName("Tier1DirectSale");
            builder.Property(t => t.Tier2SaleToTier1Sale).HasColumnName("Tier2SaleToTier1Sale");
            builder.Property(t => t.Tier3SaleToTier1Sale).HasColumnName("Tier3SaleToTier1Sale");
            builder.Property(t => t.Tier1DirectRate).HasColumnName("Tier1DirectRate");
            builder.Property(t => t.Tier2SaleToTier1Rate).HasColumnName("Tier2SaleToTier1Rate");
            builder.Property(t => t.Tier3SaleToTier1Rate).HasColumnName("Tier3SaleToTier1Rate");

            builder.HasOne(x => x.SysUser)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.SysUserId);
        }
    }
}
