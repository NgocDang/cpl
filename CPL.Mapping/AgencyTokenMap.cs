using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CPL.Mapping
{
    public class AgencyTokenMap : IEntityTypeConfiguration<AgencyToken>
    {
        public void Configure(EntityTypeBuilder<AgencyToken> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("AgencyToken");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Token).HasColumnName("Token");
            builder.Property(t => t.ExpiredDate).HasColumnName("ExpiredDate");
            builder.Property(t => t.SysUserId).HasColumnName("SysUserId");
        }
    }
}
