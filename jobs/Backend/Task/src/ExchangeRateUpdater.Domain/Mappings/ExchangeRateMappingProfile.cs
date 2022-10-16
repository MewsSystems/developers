using AutoMapper;
using ExchangeRateUpdater.Clients.Cnb.Responses;
using ExchangeRateUpdater.Domain.Mappings.ValueResolvers;
using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Domain.Mappings;

/// <summary>
/// Mapping profile for exchange rate
/// </summary>
public class ExchangeRateMappingProfile : Profile
{
    /// <summary>
    /// Constructs a <see cref="ExchangeRateMappingProfile"/>
    /// </summary>
    public ExchangeRateMappingProfile()
    {
        CreateMap<ExchangeRatesResponse, IEnumerable<ExchangeRate>>().ConvertUsing<ExchangeRateTypeConverter>();
    }
}