using AutoMapper;
using CPL.Domain;
using CPL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc.AutoMapper
{
    public class CoinTransactionProfile : Profile
    {
        public CoinTransactionProfile()
        {
            CreateMap<CoinTransaction, CoinTransactionViewModel>()
                .ForMember(dest => dest.CurrencyInString, opt => opt.MapFrom(src => src.Currency.Name));
        }
    }
}
