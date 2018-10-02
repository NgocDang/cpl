using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CPL.Mapping
{
    public class PricePredictionCategoryMap : IEntityTypeConfiguration<PricePredictionCategory>
    {
        public void Configure(EntityTypeBuilder<PricePredictionCategory> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("PricePredictionCategory");
            builder.Property(t => t.Id).HasColumnName("Id");
        }
    }
}
