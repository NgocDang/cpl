using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Mapping
{
    public class IntroducedUsersMap : IEntityTypeConfiguration<IntroducedUsers>
    {
        public void Configure(EntityTypeBuilder<IntroducedUsers> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("IntroducedUsers");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.DirectIntroducedUsers).HasColumnName("DirectIntroducedUsers");
            builder.Property(t => t.TotalDirectIntroducedUsers).HasColumnName("TotalDirectIntroducedUsers");
            builder.Property(t => t.Tier2IntroducedUsers).HasColumnName("Tier2IntroducedUsers");
            builder.Property(t => t.TotalTier2IntroducedUsers).HasColumnName("TotalTier2IntroducedUsers");
            builder.Property(t => t.Tier3IntroducedUsers).HasColumnName("Tier3IntroducedUsers");
            builder.Property(t => t.TotalTier3IntroducedUsers).HasColumnName("TotalTier3IntroducedUsers");
            builder.Property(t => t.DirectAffiliateSale).HasColumnName("DirectAffiliateSale");
            builder.Property(t => t.Tier2AffiliateSale).HasColumnName("Tier2AffiliateSale");
            builder.Property(t => t.Tier3AffiliateSale).HasColumnName("Tier3AffiliateSale");

            builder.HasOne(x => x.SysUser)
                .WithOne(x => x.IntroducedUsers)
                .IsRequired(true)
                .HasForeignKey<SysUser>(x => x.Id);
        }
    }
}
