using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Mapping
{
    public class BTCPriceMap : IEntityTypeConfiguration<BTCPrice>
    {
        public void Configure(EntityTypeBuilder<BTCPrice> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("BTCPrice");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Price).HasColumnName("Price");
            builder.Property(t => t.Time).HasColumnName("Time");
        }
    }
}
