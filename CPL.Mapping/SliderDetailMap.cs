using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Mapping
{
    public class SliderDetailMap : IEntityTypeConfiguration<SliderDetail>
    {
        public void Configure(EntityTypeBuilder<SliderDetail> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("SliderDetail");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.DesktopImage).HasColumnName("DesktopImage");
            builder.Property(t => t.MobileImage).HasColumnName("MobileImage");
            builder.Property(t => t.LangId).HasColumnName("LangId");
            builder.Property(t => t.SliderId).HasColumnName("SliderId");

            builder.HasOne(x => x.Lang)
                .WithMany(x => x.SliderDetails)
                .HasForeignKey(x => x.LangId);

            builder.HasOne(x => x.Slider)
                .WithMany(x => x.SliderDetails)
                .HasForeignKey(x => x.SliderId);
        }
    }
}
