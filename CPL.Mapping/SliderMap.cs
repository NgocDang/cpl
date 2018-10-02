using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Mapping
{
    public class SliderMap : IEntityTypeConfiguration<Slider>
    {
        public void Configure(EntityTypeBuilder<Slider> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("Slider");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Name).HasColumnName("Name");
            builder.Property(t => t.Url).HasColumnName("Url");
            builder.Property(t => t.GroupId).HasColumnName("GroupId");
            builder.Property(t => t.Status).HasColumnName("Status");

            builder.HasOne(x => x.Group)
                .WithMany(x => x.Sliders)
                .HasForeignKey(x => x.GroupId);
        }
    }
}
