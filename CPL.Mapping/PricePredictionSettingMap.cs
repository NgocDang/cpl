using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CPL.Mapping
{
    public class PricePredictionSettingMap : IEntityTypeConfiguration<PricePredictionSetting>
    {
        public void Configure(EntityTypeBuilder<PricePredictionSetting> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("PricePredictionSetting");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.OpenBettingTime).HasColumnName("OpenBettingTime");
            builder.Property(t => t.CloseBettingTime).HasColumnName("CloseBettingTime");
            builder.Property(t => t.HoldingTimeInterval).HasColumnName("HoldingTimeInterval");
            builder.Property(t => t.ResultTimeInterval).HasColumnName("ResultTimeInterval");
            builder.Property(t => t.PricePredictionCategoryId).HasColumnName("PricePredictionCategoryId");
            builder.Property(t => t.DividendRate).HasColumnName("DividendRate");
            builder.Property(t => t.Status).HasColumnName("Status");
            builder.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            builder.Property(t => t.IsDeleted).HasColumnName("IsDeleted");

            //Relationship
            builder.HasOne(x => x.PricePredictionCategory)
                .WithMany(x => x.PricePredictionSettings)
                .HasForeignKey(x => x.PricePredictionCategoryId);
        }
    }
}
