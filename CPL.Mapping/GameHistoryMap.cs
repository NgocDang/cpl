using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Mapping
{
    public class GameHistoryMap : IEntityTypeConfiguration<GameHistory>
    {
        public void Configure(EntityTypeBuilder<GameHistory> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("GameHistory");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.GameId).HasColumnName("GameId");
            builder.Property(t => t.SysUserId).HasColumnName("SysUserId");
            builder.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            builder.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(t => t.Money).HasColumnName("Money");
            builder.Property(t => t.Result).HasColumnName("Result");
            builder.Property(t => t.Bonus).HasColumnName("Bonus");

            //Relationship
            builder.HasOne(x => x.Game)
            .WithMany(x => x.GameHistories)
            .HasForeignKey(x => x.GameId);

            builder.HasOne(x => x.SysUser)
            .WithMany(x => x.GameHistorys)
            .HasForeignKey(x => x.SysUserId);
        }
    }
}
