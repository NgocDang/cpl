using AutoMapper;
using CPL.Domain;
using CPL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc.AutoMapper
{
    public class PricePredictionProfile : Profile
    {
        public PricePredictionProfile()
        {
            CreateMap<PricePrediction, PricePredictionIndexViewModel>();
            CreateMap<PricePrediction, PricePredictionViewComponentViewModel>();
            CreateMap<PricePrediction, PricePredictionTab>();
            CreateMap<PricePredictionIndexViewModel, PricePrediction>();
        }
    }
}
