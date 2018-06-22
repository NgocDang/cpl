using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Mapping
{
    public class NotificationMap : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("Notification");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Value).HasColumnName("Value");
            builder.Property(t => t.LangId).HasColumnName("LangId");
            builder.Property(t => t.CreatedDate).HasColumnName("CreatedDate");

            builder.HasOne(x => x.Lang)
                .WithMany(x => x.Notifications)
                .HasForeignKey(x => x.LangId);
        }
    }
}
