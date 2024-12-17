using AutoMapper;
using ExchangeRateUpdater.ApplicationServices.ExchangeRates.Dto;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.ApplicationServices.MapperProfiles;

public class DefaultMapperProfile : Profile
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public DefaultMapperProfile()
    {
        CreateMap<ExchangeRate, ExchangeRateDto>().ReverseMap();
        CreateMap<Currency, CurrencyDto>().ReverseMap();
    }
}
