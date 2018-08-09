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
            CreateMap<SysUser, SysUserViewModel>()
                .ForMember(dest => dest.DOBInString, opt => opt.MapFrom(src => src.DOB.HasValue ? src.DOB.Value.ToString("yyyy/MM/dd") : string.Empty))
                .ForMember(dest => dest.CreatedDateInString, opt => opt.MapFrom(src => src.CreatedDate.ToString("yyyy/MM/dd")))
                .ForMember(dest => dest.KYCCreatedDateInString, opt => opt.MapFrom(src => src.KYCCreatedDate.HasValue ? src.KYCCreatedDate.Value.ToString("yyyy/MM/dd") : string.Empty));

            CreateMap<SysUser, ActivateEmailTemplateViewModel>();
            CreateMap<SysUser, MemberEmailTemplateViewModel>();
            CreateMap<SysUser, KYCVerifyEmailTemplateViewModel>();
            CreateMap<SysUser, ForgotPasswordEmailTemplateViewModel>();
            CreateMap<SysUserViewModel, SysUser>();
            CreateMap<SysUserViewModel, EditAccountViewModel>();
            CreateMap<SysUserViewModel, EditCredentialViewModel>()
                .ForMember(dest => dest.CurrentEmail, opt => opt.MapFrom(src => src.Email));

            CreateMap<SysUserViewModel, DashboardNavbarViewModel>();
            CreateMap<SysUserViewModel, EditSecurityViewModel>();

            CreateMap<SysUser, DashboardViewModel>();
            CreateMap<SysUser, HoldingPercentageViewModel>();

            CreateMap<SysUser, ExchangeViewModel>();
            CreateMap<SysUser, TokenBalanceViewModel>();

            CreateMap<SysUser, EditAccountViewModel>();
            CreateMap<SysUser, EditSecurityViewModel>();
            CreateMap<SysUser, UserDashboardAdminViewModel>();
        }
    }
}
