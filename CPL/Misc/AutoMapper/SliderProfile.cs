using AutoMapper;
using CPL.Domain;
using CPL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc.AutoMapper
{
    public class SliderProfile : Profile
    {
        public SliderProfile()
        {
            CreateMap<Slider, SliderAdminViewModel>();
            CreateMap<SliderDetail, SliderDetailAdminViewModel>();

            CreateMap<Slider, SliderViewModel>();
            CreateMap<SliderDetail, SliderDetailViewModel>();
        }
    }
}
