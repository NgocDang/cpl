using AutoMapper;
using CPL.Domain;
using CPL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CPL.Common.Enums.CPLConstant;

namespace CPL.Misc.AutoMapper
{
    public class PricePredictionSettingProfile : Profile
    {
        public PricePredictionSettingProfile()
        {
            CreateMap<PricePredictionSettingDetail, PricePredictionSettingDetailAdminViewModel>();

            CreateMap<PricePredictionSettingDetailAdminViewModel, PricePredictionSettingDetail>()
                .ForMember(dest => dest.PricePredictionSetting, opt => opt.Ignore());
            CreateMap<PricePredictionSettingAdminViewModel, PricePredictionSetting>()
                .ForMember(dest => dest.PricePredictionSettingDetails, opt => opt.Ignore());

            CreateMap<PricePredictionSetting, PricePredictionSettingAdminViewModel>()
                .ForMember(dest => dest.BettingTimeInString, opt => opt.MapFrom(src => src.OpenBettingTime.ToString(Format.Time) + " - " + src.CloseBettingTime.ToString(Format.Time)))
                .ForMember(dest => dest.PricePredictionCategories, opt => opt.Ignore());
        }
    }
}
