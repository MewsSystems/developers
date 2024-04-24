namespace ExchangeRateUpdater.Application.Common.Mappings.Profiles;

using AutoMapper;
using ExchangeRateUpdater.Application.ExchangeRates.Dtos;
using ExchangeRateUpdater.Domain.Entities;

public class ExchangeRateDtoProfile : Profile
{
    public ExchangeRateDtoProfile()
    {
        CreateMap<string, Currency>()
            .ConvertUsing(s => new Currency(s));
        CreateMap<ExchangeRate, ExchangeRateDto>()
            .ForMember(dest => dest.SourceCurrencyCode, opt => opt.MapFrom(src => src.SourceCurrency.Code))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
            .ForMember(dest => dest.TargetCurrencyCode, opt => opt.MapFrom(src => "CZK"));
    }
}