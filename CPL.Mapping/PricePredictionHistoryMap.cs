using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Mapping
{
    public class PricePredictionHistoryMap : IEntityTypeConfiguration<PricePredictionHistory>
    {
        public void Configure(EntityTypeBuilder<PricePredictionHistory> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("PricePredictionHistory");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.PricePredictionId).HasColumnName("PricePredictionId");
            builder.Property(t => t.SysUserId).HasColumnName("SysUserId");
            builder.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            builder.Property(t => t.Amount).HasColumnName("Amount");
            builder.Property(t => t.Prediction).HasColumnName("Prediction");
            builder.Property(t => t.Result).HasColumnName("Result");
            builder.Property(t => t.Award).HasColumnName("Award");
            builder.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");

            //Relationship
            builder.HasOne(x => x.PricePrediction)
                .WithMany(x => x.PricePredictionHistories)
                .HasForeignKey(x => x.PricePredictionId);

            builder.HasOne(x => x.SysUser)
                .WithMany(x => x.PricePredictionHistories)
                .HasForeignKey(x => x.SysUserId);
        }
    }
}
