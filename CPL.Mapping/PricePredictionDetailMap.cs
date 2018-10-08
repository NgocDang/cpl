using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CPL.Mapping
{
    public class PricePredictionDetailMap : IEntityTypeConfiguration<PricePredictionDetail>
    {
        public void Configure(EntityTypeBuilder<PricePredictionDetail> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("PricePredictionDetail");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Title).HasColumnName("Title");
            builder.Property(t => t.LangId).HasColumnName("LangId");
            builder.Property(t => t.ShortDescription).HasColumnName("ShortDescription");
            builder.Property(t => t.PricePredictionId).HasColumnName("PricePredictionId");

            //Relationship
            builder.HasOne(x => x.Lang)
                .WithMany(x => x.PricePredictionDetails)
                .HasForeignKey(x => x.LangId);

            builder.HasOne(x => x.PricePrediction)
                .WithMany(x => x.PricePredictionDetails)
                .HasForeignKey(x => x.PricePredictionId);
        }
    }
}
