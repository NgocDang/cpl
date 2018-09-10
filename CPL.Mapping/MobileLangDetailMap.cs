using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Mapping
{
    public class MobileLangDetailMap : IEntityTypeConfiguration<MobileLangDetail>
    {
        public void Configure(EntityTypeBuilder<MobileLangDetail> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("LangDetail");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.LangId).HasColumnName("LangId");
            builder.Property(t => t.Name).HasColumnName("Name");
            builder.Property(t => t.Value).HasColumnName("Value");

            builder.HasOne(x => x.Lang)
                .WithMany(x => x.MobileLangDetails)
                .HasForeignKey(x => x.LangId);
        }
    }
}
