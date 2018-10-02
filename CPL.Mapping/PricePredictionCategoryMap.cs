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
            builder.Property(t => t.Name).HasColumnName("Name");
            builder.Property(t => t.LangId).HasColumnName("LangId");
            builder.Property(t => t.Description).HasColumnName("Description");

            //Relationship
            builder.HasOne(x => x.Lang)
                .WithMany(x => x.PricePredictionCategories)
                .HasForeignKey(x => x.LangId);
        }
    }
}
