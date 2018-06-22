using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Mapping
{
    public class TeamMap : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("Team");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Designation).HasColumnName("Designation");
            builder.Property(t => t.Name).HasColumnName("Name");
            builder.Property(t => t.Avatar).HasColumnName("Avatar");
        }
    }
}
