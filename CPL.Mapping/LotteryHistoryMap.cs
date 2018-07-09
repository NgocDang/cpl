using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Mapping
{
    public class LotteryHistoryMap : IEntityTypeConfiguration<LotteryHistory>
    {
        public void Configure(EntityTypeBuilder<LotteryHistory> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("LotteryHistory");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.LotteryId).HasColumnName("LotteryId");
            builder.Property(t => t.SysUserId).HasColumnName("SysUserId");
            builder.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            builder.Property(t => t.Result).HasColumnName("Result");
            builder.Property(t => t.TicketNumber).HasColumnName("TicketNumber");
            builder.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(t => t.Price).HasColumnName("Price");
            builder.Property(t => t.PrizeId).HasColumnName("PrizeId");
            builder.Property(t => t.TicketAmount).HasColumnName("TicketAmount");

            //Relationship
            builder.HasOne(x => x.Lottery)
                .WithMany(x => x.LotteryHistories)
                .HasForeignKey(x => x.LotteryId);

            builder.HasOne(x => x.LotteryPrize)
                .WithMany(x => x.LotteryHistories)
                .HasForeignKey(x => x.PrizeId);
        }
    }
}
