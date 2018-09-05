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
            CreateMap<SysUser, AffiliateApproveEmailTemplateViewModel>();

            CreateMap<SysUserViewModel, SysUser>();
            CreateMap<SysUserViewModel, ProfileViewModel>();
            CreateMap<SysUserViewModel, SecurityViewModel>()
                .ForMember(dest => dest.CurrentEmail, opt => opt.MapFrom(src => src.Email));

            CreateMap<SysUserViewModel, DashboardNavbarViewModel>();
            CreateMap<SysUserViewModel, KYCViewModel>();

            CreateMap<SysUser, ProfileAffiliateViewModel>();

            CreateMap<SysUser, DashboardViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
            CreateMap<SysUser, BalanceViewModel>();
            CreateMap<SysUser, RateViewModel>();
            CreateMap<SysUser, HoldingPercentageViewModel>();

            CreateMap<SysUser, ExchangeViewModel>();
            CreateMap<SysUser, LotteryResultViewModel>();

            CreateMap<SysUser, ProfileViewModel>();
            CreateMap<SysUser, KYCViewModel>();
            CreateMap<SysUser, UserDashboardAdminViewModel>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.StreetAddress} {(!string.IsNullOrWhiteSpace(src.StreetAddress) ? "," : "")} {src.City + (!string.IsNullOrWhiteSpace(src.City) ? "," : "")} {src.Country}"));
            CreateMap<SysUser, GameHistoryViewModel>();
            CreateMap<SysUser, GameHistoryIndexViewModel>();
            CreateMap<SysUser, UserLotteryPrizeViewModel>();
        }
    }
}
