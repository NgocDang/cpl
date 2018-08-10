using AutoMapper;
using CPL.Common.Enums;
using CPL.Domain;
using CPL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc.AutoMapper
{
    public class PricePredictionHistoryProfile : Profile
    {
        public PricePredictionHistoryProfile()
        {
            CreateMap<PricePredictionHistory, PricePredictionHistoryViewModel>();
            CreateMap<PricePredictionHistoryViewModel, PricePredictionHistory>();
            CreateMap<PricePredictionHistory, GameHistoryViewModel>()
                .ForMember(dest => dest.GameType, opt => opt.MapFrom(src => CPLConstant.PricePredictionName))
                .ForMember(dest => dest.AmountInString, opt => opt.MapFrom(src => src.Amount.ToString("#,##0")))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.AwardInString, opt => opt.MapFrom(src => src.Award.HasValue ? src.Award.Value.ToString("#,##0") : string.Empty))
                .ForMember(dest => dest.Award, opt => opt.MapFrom(src => src.Award.HasValue ? src.Award.Value : 0))
                .ForMember(dest => dest.CreatedDateInString, opt => opt.MapFrom(src => src.CreatedDate.ToString("yyyy/MM/dd")))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.CreatedTimeInString, opt => opt.MapFrom(src => src.CreatedDate.ToString("HH:mm:ss")))
                .ForMember(dest => dest.Result, opt => opt.MapFrom(src => src.Result))
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Award.HasValue ? (src.Award.Value - src.Amount) : 0))
                .ForMember(dest => dest.BalanceInString, opt => opt.MapFrom(src => src.Award.HasValue ? (src.Award.Value - src.Amount).ToString("+#,##0;-#,##0") : string.Empty));
            CreateMap<GameHistoryViewModel, PricePredictionHistory>();
        }
    }
}
