using CPL.Domain;
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
            builder.Property(t => t.Volume).HasColumnName("Volume");
        }
    }
}
