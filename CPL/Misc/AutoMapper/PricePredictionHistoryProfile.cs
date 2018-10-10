using AutoMapper;
using CPL.Common.Enums;
using CPL.Domain;
using CPL.Misc.Utils;
using CPL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CPL.Common.Enums.CPLConstant;

namespace CPL.Misc.AutoMapper
{
    public class PricePredictionHistoryProfile : Profile
    {
        public PricePredictionHistoryProfile()
        {
            CreateMap<PricePredictionHistory, PricePredictionHistoryViewModel>()
                .ForMember(dest => dest.ToBeComparedPrice, opt => opt.MapFrom(src => src.PricePrediction.ToBeComparedPrice))
                .ForMember(dest => dest.ToBeComparedPriceInString, opt => opt.MapFrom(src => $"{src.PricePrediction.ToBeComparedPrice.GetValueOrDefault(0).ToString(Format.Amount)} {EnumCurrency.USDT.ToString()}"))
                .ForMember(dest => dest.CurrencyPair, opt => opt.MapFrom(src => src.PricePrediction.Coinbase))
                .ForMember(dest => dest.CurrencyPairInString, opt => opt.MapFrom(src => EnumHelper<EnumCurrencyPair>.GetDisplayValue((EnumCurrencyPair)Enum.Parse(typeof(EnumCurrencyPair), src.PricePrediction.Coinbase))))
                .ForMember(dest => dest.ResultPrice, opt => opt.MapFrom(src => src.PricePrediction.ResultPrice))
                .ForMember(dest => dest.ResultPriceInString, opt => opt.MapFrom(src => $"{src.PricePrediction.ResultPrice.GetValueOrDefault(0).ToString(Format.Amount)} {EnumCurrency.USDT.ToString()}"))
                .ForMember(dest => dest.ResultTime, opt => opt.MapFrom(src => src.PricePrediction.ResultTime))
                .ForMember(dest => dest.ResultTimeInString, opt => opt.MapFrom(src => src.PricePrediction.ResultTime.ToString(Format.DateTime)))
                .ForMember(dest => dest.Bet, opt => opt.MapFrom(src => src.Prediction == true ? EnumPricePredictionStatus.UP.ToString() : EnumPricePredictionStatus.DOWN.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ((EnumPricePredictionGameStatus)src.PricePrediction.Status).ToString()))
                .ForMember(dest => dest.PurcharseTime, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.PurcharseTimeInString, opt => opt.MapFrom(src => $"{src.CreatedDate.ToString(Format.DateTime)}"))
                .ForMember(dest => dest.Bonus, opt => opt.MapFrom(src => src.Award.GetValueOrDefault(0)))
                .ForMember(dest => dest.BonusInString, opt => opt.MapFrom(src => $"{src.Award.GetValueOrDefault(0).ToString(Format.Amount)} {EnumCurrency.CPL.ToString()}"))
                .ForMember(dest => dest.AmountInString, opt => opt.MapFrom(src => $"{src.Amount.ToString(Format.Amount)} {EnumCurrency.CPL.ToString()}"));
            CreateMap<PricePredictionHistoryViewModel, PricePredictionHistory>();
            CreateMap<PricePredictionHistory, GameHistoryViewModel>()
                .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.PricePredictionId))
                .ForMember(dest => dest.GameType, opt => opt.MapFrom(src => EnumGameType.PRICE_PREDICTION))
                .ForMember(dest => dest.AmountInString, opt => opt.MapFrom(src => src.Amount.ToString("#,##0")))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.AwardInString, opt => opt.MapFrom(src => src.TotalAward.HasValue ? src.TotalAward.Value.ToString("#,##0") : string.Empty))
                .ForMember(dest => dest.Award, opt => opt.MapFrom(src => src.TotalAward.HasValue ? src.TotalAward.Value : 0))
                .ForMember(dest => dest.CreatedDateInString, opt => opt.MapFrom(src => src.CreatedDate.ToString("yyyy/MM/dd")))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.CreatedTimeInString, opt => opt.MapFrom(src => src.CreatedDate.ToString("HH:mm:ss")))
                .ForMember(dest => dest.Result, opt => opt.MapFrom(src => src.Result))
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.TotalAward.HasValue ? (src.TotalAward.Value - src.Amount) : 0))
                .ForMember(dest => dest.BalanceInString, opt => opt.MapFrom(src => src.TotalAward.HasValue ? (src.TotalAward.Value - src.Amount).ToString("+#,##0;-#,##0") : string.Empty));
            CreateMap<GameHistoryViewModel, PricePredictionHistory>();
        }
    }
}
