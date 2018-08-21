using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Mapping
{
    public class LangContentMap : IEntityTypeConfiguration<LangContent>
    {
        public void Configure(EntityTypeBuilder<LangContent> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("LangContent");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.FieldName).HasColumnName("FieldName");
            builder.Property(t => t.LangId).HasColumnName("LangId");
            builder.Property(t => t.RowId).HasColumnName("RowId");
            builder.Property(t => t.TableName).HasColumnName("TableName");
            builder.Property(t => t.Value).HasColumnName("Value");
        }
    }
}
