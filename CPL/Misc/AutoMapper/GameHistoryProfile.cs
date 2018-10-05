using AutoMapper;
using CPL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static CPL.Common.Enums.CPLConstant;

namespace CPL.Misc.AutoMapper
{
    public class GameHistoryProfile : Profile
    {
        public GameHistoryProfile()
        {
            CreateMap<DataRow, GameHistoryViewModel>()
                .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => Convert.ToInt32(src["GameId"])))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src["CreatedDate"])))
                .ForMember(dest => dest.CreatedDateInString, opt => opt.MapFrom(src => Convert.ToString(src["CreatedDateInString"])))
                .ForMember(dest => dest.CreatedTimeInString, opt => opt.MapFrom(src => Convert.ToString(src["CreatedTimeInString"])))
                .ForMember(dest => dest.GameType, opt => opt.MapFrom(src => Convert.ToString(src["GameType"])))
                
                .ForMember(dest => dest.Result, opt => opt.MapFrom(src => Convert.ToString(src["Result"])))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => Convert.ToDecimal(src["Amount"])))
                .ForMember(dest => dest.AmountInString, opt => opt.MapFrom(src => Convert.ToDecimal(src["Amount"]).ToString(Format.Number)))
                .ForMember(dest => dest.Award, opt => opt.MapFrom(src => Convert.ToDecimal(src["Award"])))
                .ForMember(dest => dest.AwardInString, opt => opt.MapFrom(src => Convert.ToDecimal(src["Award"]).ToString(Format.Number)))
                .ForMember(dest => dest.BalanceInString, opt => opt.MapFrom(src => Convert.ToInt32(src["Balance"]).ToString("+#,##0;-#,##0")))
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => (Convert.ToInt32(src["Balance"]))));
        }
    }
}
