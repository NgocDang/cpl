using AutoMapper;
using CPL.Domain;
using CPL.Models;

namespace CPL.Misc.AutoMapper
{
    public class AgencyProfile : Profile
    {
        public AgencyProfile()
        {
            CreateMap<AgencyAffiliateRateViewModel, Agency>();
        }
    }
}
