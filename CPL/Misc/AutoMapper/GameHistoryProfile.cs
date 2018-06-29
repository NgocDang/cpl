using AutoMapper;
using CPL.Domain;
using CPL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc.AutoMapper
{
    public class GameHistoryProfile : Profile
    {
        public GameHistoryProfile()
        {
            CreateMap<GameHistory, GameHistoryViewModel>()
                .ForMember(dest => dest.AmountInString, opt => opt.MapFrom(src => src.Amount.ToString("#,##0.########")))
                .ForMember(dest => dest.BonusInString, opt => opt.MapFrom(src => src.Award.HasValue ? src.Award.Value.ToString("#,##0.########") : string.Empty))
                .ForMember(dest => dest.CreatedDateInString, opt => opt.MapFrom(src => src.CreatedDate.ToString("yyyy/MM/dd")))
                .ForMember(dest => dest.CreatedTimeInString, opt => opt.MapFrom(src => src.CreatedDate.ToString("HH:mm:ss")))
                .ForMember(dest => dest.BalanceInString, opt => opt.MapFrom(src => src.Award.HasValue ?
                    ((src.Award.Value - src.Amount >= 0) ? (src.Award.Value - src.Amount).ToString("+#,##0.########") : (src.Award.Value - src.Amount).ToString("#,##0.########"))
                    : string.Empty)
                    );

            CreateMap<GameHistoryViewModel, GameHistory>();
        }
    }
}
