using CPL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Mapping
{
    public class SysUserMap : IEntityTypeConfiguration<SysUser>
    {
        public void Configure(EntityTypeBuilder<SysUser> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("SysUser");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.IsAdmin).HasColumnName("IsAdmin");
            builder.Property(t => t.Email).HasColumnName("Email");
            builder.Property(t => t.Password).HasColumnName("Password");
            builder.Property(t => t.FirstName).HasColumnName("FirstName");
            builder.Property(t => t.LastName).HasColumnName("LastName");
            builder.Property(t => t.Address).HasColumnName("Address");
            builder.Property(t => t.ETHWalletAddress).HasColumnName("ETHWalletAddress");
            builder.Property(t => t.BTCWalletAddress).HasColumnName("BTCWalletAddress");
            builder.Property(t => t.ETHHDWalletAddress).HasColumnName("ETHHDWalletAddress");
            builder.Property(t => t.ETHHDWalletAddressIndex).HasColumnName("ETHHDWalletAddressIndex");
            builder.Property(t => t.BTCHDWalletAddress).HasColumnName("BTCHDWalletAddress");
            builder.Property(t => t.BTCHDWalletAddressIndex).HasColumnName("BTCHDWalletAddressIndex");
            builder.Property(t => t.Mobile).HasColumnName("Mobile");
            builder.Property(t => t.ResetPasswordDate).HasColumnName("ResetPasswordDate");
            builder.Property(t => t.ResetPasswordToken).HasColumnName("ResetPasswordToken");
            builder.Property(t => t.ActivateToken).HasColumnName("ActivateToken");
            builder.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            builder.Property(t => t.KYCVerified).HasColumnName("KYCVerified");
            builder.Property(t => t.KYCCreatedDate).HasColumnName("KYCCreatedDate");
            builder.Property(t => t.FrontSide).HasColumnName("FrontSide");
            builder.Property(t => t.BackSide).HasColumnName("BackSide");
            builder.Property(t => t.DOB).HasColumnName("DOB");
            builder.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            builder.Property(t => t.BTCAmount).HasColumnName("BTCAmount");
            builder.Property(t => t.ETHAmount).HasColumnName("ETHAmount");
            builder.Property(t => t.TokenAmount).HasColumnName("TokenAmount");
        }
    }
}
