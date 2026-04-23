using AutoMapper;
using Mews.ExchangeRateUpdater.Application.ExchangeRates.Dto;
using Mews.ExchangeRateUpdater.Domain.Entities.ExchangeRateAgg;
using System.Diagnostics.CodeAnalysis;

namespace Mews.ExchangeRateUpdater.Application.MapperProfiles;

/// <summary>
/// Default Mapper Profile.
/// </summary>
[ExcludeFromCodeCoverage]
public class DefaultMapperProfile : Profile
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public DefaultMapperProfile()
    {
        _ = CreateMap<ExchangeRate, ExchangeRateDto>().ReverseMap();
        _ = CreateMap<Currency, CurrencyDto>().ReverseMap();
    }
}
