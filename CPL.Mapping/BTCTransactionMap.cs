using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Mapping
{
    public class BTCTransactionMap : IEntityTypeConfiguration<BTCTransaction>
    {
        public void Configure(EntityTypeBuilder<BTCTransaction> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("BTCTransaction");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.TxId).HasColumnName("TxId");
            builder.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
    }
}
