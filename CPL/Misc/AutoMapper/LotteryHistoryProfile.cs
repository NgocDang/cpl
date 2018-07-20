using AutoMapper;
using CPL.Domain;
using CPL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc.AutoMapper
{
    public class LotteryHistoryProfile : Profile
    {
        public LotteryHistoryProfile()
        {
            CreateMap<LotteryHistory, LotteryHistoryViewModel>();
            CreateMap<LotteryHistoryViewModel, LotteryHistory>();
        }
    }
}
