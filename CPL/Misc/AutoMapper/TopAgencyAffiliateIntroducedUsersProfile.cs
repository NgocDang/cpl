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
    public class TopAgencyAffiliateIntroducedUsersProfile: Profile
    {
        public TopAgencyAffiliateIntroducedUsersProfile()
        {
            CreateMap<DataRow, TopAgencyAffiliateIntroducedUsersViewModel>()
                     .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Convert.ToInt32(src["Id"])))
                     .ForMember(dest => dest.KindOfTier, opt => opt.MapFrom(src => Convert.ToString(src["KindOfTier"])))
                     .ForMember(dest => dest.UsedCPL, opt => opt.MapFrom(src => Convert.ToDecimal(src["UsedCPL"])))
                     .ForMember(dest => dest.LostCPL, opt => opt.MapFrom(src => Convert.ToDecimal(src["LostCPL"])))
                     .ForMember(dest => dest.AffiliateSale, opt => opt.MapFrom(src => Convert.ToDecimal(src["AffiliateSale"])))
                     .ForMember(dest => dest.TotalDirectIntroducedUsers, opt => opt.MapFrom(src => Convert.ToInt32(src["TotalDirectIntroducedUsers"])))
                     .ForMember(dest => dest.AffiliateCreatedDateInString, opt => opt.MapFrom(src => (Convert.ToDateTime(src["AffiliateCreatedDate"])).ToString(Format.DateTime)));
        }
    }
}
