using AutoMapper;
using CPL.Domain;
using CPL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc.MapperCreate
{
    public class SysUserProfile : Profile
    {
        public SysUserProfile()
        {
            CreateMap<SysUser, SysUserViewModel>();
            CreateMap<SysUser, ActivateEmailTemplateViewModel>();
            CreateMap<SysUser, MemberEmailTemplateViewModel>();

            CreateMap<SysUserViewModel, SysUser>();
            CreateMap<SysUserViewModel, EditAccountViewModel>();
            CreateMap<SysUserViewModel, EditCredentialViewModel>()
                .ForMember(dest => dest.CurrentEmail, opt => opt.MapFrom(src => src.Email));

            CreateMap<SysUserViewModel, DashboardNavbarViewModel>();
            CreateMap<SysUserViewModel, EditSecurityViewModel>();

            CreateMap<SysUser, DashboardViewModel>();
            CreateMap<SysUser, HoldingPercentageViewModel>();
            CreateMap<GameHistory, GameHistoryViewModel>();

            CreateMap<SysUser, ExchangeViewModel>();
            CreateMap<SysUser, TokenBalanceViewModel>();
        }
    }
}
