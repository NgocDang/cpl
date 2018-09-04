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
            builder.Property(t => t.Name).HasColumnName("Name");
            builder.Property(t => t.Description).HasColumnName("Description");
            builder.Property(t => t.OpenTime).HasColumnName("OpenTime");
            builder.Property(t => t.EndTime).HasColumnName("EndTime");
            builder.Property(t => t.PredictionResultTime).HasColumnName("PredictionResultTime");
            builder.Property(t => t.PredictionPrice).HasColumnName("PredictionPrice");
            builder.Property(t => t.ResultPrice).HasColumnName("ResultPrice");
            builder.Property(t => t.NumberOfPredictors).HasColumnName("NumberOfPredictors");
            builder.Property(t => t.Volume).HasColumnName("Volume");
            builder.Property(t => t.Coinbase).HasColumnName("Coinbase");
            builder.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
        }
    }
}
