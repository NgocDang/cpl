using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CPL.Mapping
{
    public class PricePredictionSettingDetailMap : IEntityTypeConfiguration<PricePredictionSettingDetail>
    {
        public void Configure(EntityTypeBuilder<PricePredictionSettingDetail> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("PricePredictionSettingDetail");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Title).HasColumnName("Title");
            builder.Property(t => t.LangId).HasColumnName("LangId");
            builder.Property(t => t.PricePredictionSettingId).HasColumnName("PricePredictionSettingId");

            //Relationship
            builder.HasOne(x => x.Lang)
                .WithMany(x => x.PricePredictionSettingDetails)
                .HasForeignKey(x => x.LangId);

            builder.HasOne(x => x.PricePredictionSetting)
                .WithMany(x => x.PricePredictionSettingDetails)
                .HasForeignKey(x => x.PricePredictionSettingId);
        }
    }
}
