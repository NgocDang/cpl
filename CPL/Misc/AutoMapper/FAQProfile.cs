using AutoMapper;
using CPL.Domain;
using CPL.Models;

namespace CPL.Misc.AutoMapper
{
    public class FAQProfile : Profile
    {
        public FAQProfile()
        {
            CreateMap<FAQ, FAQViewModel>();
        }
    }
}
