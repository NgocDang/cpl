using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CPL.Mapping
{
    public class RateMap : IEntityTypeConfiguration<Rate>
    {
        public void Configure(EntityTypeBuilder<Rate> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("Rate");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Value).HasColumnName("Value");
            builder.Property(t => t.StartDate).HasColumnName("StartDate");
            builder.Property(t => t.EndDate).HasColumnName("EndDate");
            builder.Property(t => t.CurrencyId).HasColumnName("CurrencyId");

            //Relationship
            builder.HasOne(x => x.Currency)
                .WithMany(x => x.Rates)
                .HasForeignKey(x => x.CurrencyId);
        }
    }
}
