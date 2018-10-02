using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CPL.Mapping
{
    public class PricePredictionCategoryDetailMap : IEntityTypeConfiguration<PricePredictionCategoryDetail>
    {
        public void Configure(EntityTypeBuilder<PricePredictionCategoryDetail> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("PricePredictionCategoryDetail");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Name).HasColumnName("Name");
            builder.Property(t => t.Description).HasColumnName("Description");

            builder.Property(t => t.LangId).HasColumnName("LangId");
            builder.Property(t => t.PricePredictionCategoryId).HasColumnName("PricePredictionCategoryId");

            //Relationship
            builder.HasOne(x => x.Lang)
                .WithMany(x => x.PricePredictionCategoryDetails)
                .HasForeignKey(x => x.LangId);

            builder.HasOne(x => x.PricePredictionCategory)
                .WithMany(x => x.PricePredictionCategoryDetails)
                .HasForeignKey(x => x.PricePredictionCategoryId);
        }
    }
}
