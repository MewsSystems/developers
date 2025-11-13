using ExchangeRateService.Domain;
using ExchangeRateService.ExternalServices;

namespace ExchangeRateService.Mapping;

internal class ExchangeRateProfile : Profile
{
    public ExchangeRateProfile()
    {
        CreateMap<string, Currency>()
            .ConvertUsing(s => new Currency(s));
        CreateMap<ExchangeRate, string>()
            .ConvertUsing(rate => rate.ToString());
        CreateMap<CNBDailyExrate, ExchangeRate>()
            .ForMember(dest => dest.SourceCurrency, opt => opt.MapFrom(src => src.CurrencyCode))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Rate/src.Amount))
            .ForMember(dest => dest.TargetCurrency, opt => opt.MapFrom(src => "CZK"));
    }
}