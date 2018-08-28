using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Mapping
{
    public class ETHTransactionMap : IEntityTypeConfiguration<ETHTransaction>
    {
        public void Configure(EntityTypeBuilder<ETHTransaction> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("ETHTransaction");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.TxHashId).HasColumnName("TxHashId");
            builder.Property(t => t.UpdatedTime).HasColumnName("UpdatedTime");
            builder.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            builder.Property(t => t.Status).HasColumnName("Status");
            builder.Property(t => t.ParentId).HasColumnName("ParentId");
        }
    }
}
