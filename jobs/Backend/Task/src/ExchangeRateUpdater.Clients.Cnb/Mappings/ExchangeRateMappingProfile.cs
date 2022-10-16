using AutoMapper;
using ExchangeRateUpdater.Clients.Cnb.Mappings.ValueResolvers;
using ExchangeRateUpdater.Clients.Cnb.Responses;
using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Clients.Cnb.Mappings;

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
        CreateMap<ExchangeRatesResponse, IEnumerable<ExchangeRate>>().ConvertUsing<ExchangeRateValueResolver>();
    }
}