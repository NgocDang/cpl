using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Mapping
{
    public class PricePredictionMap : IEntityTypeConfiguration<PricePrediction>
    {
        public void Configure(EntityTypeBuilder<PricePrediction> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("PricePrediction");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.OpenBettingTime).HasColumnName("OpenBettingTime");
            builder.Property(t => t.CloseBettingTime).HasColumnName("CloseBettingTime");
            builder.Property(t => t.ResultTime).HasColumnName("ResultTime");
            builder.Property(t => t.ToBeComparedTime).HasColumnName("ToBeComparedTime");
            builder.Property(t => t.ResultPrice).HasColumnName("ResultPrice");
            builder.Property(t => t.ToBeComparedPrice).HasColumnName("ToBeComparedPrice");
            builder.Property(t => t.NumberOfPredictors).HasColumnName("NumberOfPredictors");
            builder.Property(t => t.Volume).HasColumnName("Volume");
            builder.Property(t => t.Coinbase).HasColumnName("Coinbase");
            builder.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(t => t.PricePredictionCategoryId).HasColumnName("PricePredictionCategoryId");
            builder.Property(t => t.IsCreatedByAdmin).HasColumnName("IsCreatedByAdmin");

            //Relationship
            builder.HasOne(x => x.PricePredictionCategory)
                .WithMany(x => x.PricePredictions)
                .HasForeignKey(x => x.PricePredictionCategoryId);
        }
    }
}
