using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Mapping
{
    public class FAQMap : IEntityTypeConfiguration<FAQ>
    {
        public void Configure(EntityTypeBuilder<FAQ> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("FAQ");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Question).HasColumnName("Question");
            builder.Property(t => t.Answer).HasColumnName("Answer");
            builder.Property(t => t.LangId).HasColumnName("LangId");
            builder.Property(t => t.GroupId).HasColumnName("GroupId");

            builder.HasOne(x => x.Lang)
                .WithMany(x => x.FAQs)
                .HasForeignKey(x => x.LangId);

            builder.HasOne(x => x.Group)
                .WithMany(x => x.FAQs)
                .HasForeignKey(x => x.GroupId);
        }
    }
}
