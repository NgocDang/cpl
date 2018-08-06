using AutoMapper;
using CPL.Domain;
using CPL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc.AutoMapper
{
    public class LotteryProfile : Profile
    {
        public LotteryProfile()
        {
            CreateMap<Lottery, LotteryViewModel>();
            CreateMap<LotteryViewModel, Lottery>();
            CreateMap<Lottery, HomeLotteryViewModel>()
                .ForMember(dest => dest.NumberOfTicketLeft, opt => opt.MapFrom(src => src.Volume - src.LotteryHistories.Count));
            CreateMap<Lottery, HomeSlideViewModel>();
        }
    }
}
