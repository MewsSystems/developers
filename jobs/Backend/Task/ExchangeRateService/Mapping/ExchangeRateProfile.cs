using ExchangeRateService.Domain;

namespace ExchangeRateService.Mapping;

internal class ExchangeRateProfile : Profile
{
    public ExchangeRateProfile()
    {
        CreateMap<string, Currency>()
            .ConvertUsing(s => new Currency(s));
        CreateMap<ExchangeRate, string>()
            .ConvertUsing(rate => rate.ToString());
    }
}