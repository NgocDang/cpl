using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CPL.Mapping
{
    public class NewsMap : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("News");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Title).HasColumnName("Title");
            builder.Property(t => t.ShortDescription).HasColumnName("ShortDescription");
            builder.Property(t => t.Description).HasColumnName("Description");
            builder.Property(t => t.Image).HasColumnName("Image");
            builder.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
        }
    }
}
