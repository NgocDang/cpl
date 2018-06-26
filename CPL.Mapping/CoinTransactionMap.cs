using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Mapping
{
    public class CoinTransactionMap : IEntityTypeConfiguration<CoinTransaction>
    {
        public void Configure(EntityTypeBuilder<CoinTransaction> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("CoinTransaction");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.SysUserId).HasColumnName("SysUserId");
            builder.Property(t => t.FromWalletAddress).HasColumnName("FromWalletAddress");
            builder.Property(t => t.ToWalletAddress).HasColumnName("ToWalletAddress");
            builder.Property(t => t.CoinAmount).HasColumnName("CoinAmount");
            builder.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            builder.Property(t => t.CurrencyId).HasColumnName("CurrencyId");
            builder.Property(t => t.Status).HasColumnName("Status");
            builder.Property(t => t.TokenAmount).HasColumnName("TokenAmount");
            builder.Property(t => t.Rate).HasColumnName("Rate");

            //Relationship
            builder.HasOne(x => x.SysUser)
            .WithMany(x => x.CoinTransactions)
            .HasForeignKey(x => x.SysUserId);

            builder.HasOne(x => x.Currency)
            .WithMany(x => x.CoinTransactions)
            .HasForeignKey(x => x.CurrencyId);
        }
    }
}
