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
    public class StandardAffiliateIntroducedUsersProfile: Profile
    {
        public StandardAffiliateIntroducedUsersProfile()
        {
            CreateMap<DataRow, StandardAffiliateIntroducedUsersViewModel>()
                     .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Convert.ToInt32(src["Id"])))
                     .ForMember(dest => dest.KindOfTier, opt => opt.MapFrom(src => Convert.ToString(src["KindOfTier"])))
                     .ForMember(dest => dest.UsedCPL, opt => opt.MapFrom(src => Convert.ToDecimal(src["UsedCPL"])))
                     .ForMember(dest => dest.LostCPL, opt => opt.MapFrom(src => Convert.ToDecimal(src["LostCPL"])))
                     .ForMember(dest => dest.AffiliateSale, opt => opt.MapFrom(src => Convert.ToDecimal(src["AffiliateSale"])))
                     .ForMember(dest => dest.TotalDirectIntroducedUsers, opt => opt.MapFrom(src => Convert.ToInt32(src["TotalIntroducedUsers"])))
                     .ForMember(dest => dest.AffiliateCreatedDateInString, opt => opt.MapFrom(src => (Convert.ToDateTime(src["AffiliateCreatedDate"])).ToString(Format.DateTime)))
                     .ForMember(dest => dest.Tier1DirectRate, opt => opt.MapFrom(src => Convert.ToInt32(src["Tier1DirectRate"])))
                     .ForMember(dest => dest.Tier2SaleToTier1Rate, opt => opt.MapFrom(src => Convert.ToInt32(src["Tier2SaleToTier1Rate"])))
                     .ForMember(dest => dest.Tier3SaleToTier1Rate, opt => opt.MapFrom(src => Convert.ToInt32(src["Tier3SaleToTier1Rate"])))
                     .ForMember(dest => dest.IsLocked, opt => opt.MapFrom(src => Convert.ToInt32(src["IsLocked"])));
        }
}
}
