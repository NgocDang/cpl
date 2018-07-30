using AutoMapper;
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
        }
    }
}
