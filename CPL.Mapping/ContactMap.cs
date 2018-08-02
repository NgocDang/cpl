using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Mapping
{
    public class ContactMap : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("Contact");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Email).HasColumnName("Email");
            builder.Property(t => t.Category).HasColumnName("Type");
            builder.Property(t => t.Description).HasColumnName("Description");
            builder.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
        }
    }
}
