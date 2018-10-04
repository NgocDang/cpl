using AutoMapper;
using CPL.Domain;
using CPL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc.AutoMapper
{
    public class DepositAndWithdrawProfile : Profile
    {
        public DepositAndWithdrawProfile()
        {
            CreateMap<SysUser, DepositAndWithdrawViewComponentViewModel>()
                .ForMember(dest => dest.BTCQrCodeImage, opt => opt.MapFrom(src => $"https://chart.googleapis.com/chart?chs=300x300&cht=qr&chl={src.BTCHDWalletAddress}&choe=UTF-8"))
                .ForMember(dest => dest.ETHQrCodeImage, opt => opt.MapFrom(src => $"https://chart.googleapis.com/chart?chs=300x300&cht=qr&chl={src.ETHHDWalletAddress}&choe=UTF-8"));
        }
    }
}
