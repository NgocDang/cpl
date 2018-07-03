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
            CreateMap<SysUser, DepositAndWithdrawViewModel>()
                .ForMember(dest => dest.BTCQrCodeImage, opt => opt.MapFrom(src => $"https://blockchain.info/qr?data=bitcoin:{src.BTCHDWalletAddress}"))
                .ForMember(dest => dest.ETHQrCodeImage, opt => opt.MapFrom(src => $"https://chart.googleapis.com/chart?chs=300x300&cht=qr&chl={src.ETHHDWalletAddress}&choe=UTF-8"));
        }
    }
}
