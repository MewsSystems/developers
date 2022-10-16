using AutoMapper;
using ExchangeRateUpdater.Clients.Cnb.Responses;
using ExchangeRateUpdater.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.Clients.Cnb.Mappings.ValueResolvers;

public class ExchangeRateValueResolver : ITypeConverter<ExchangeRatesResponse, IEnumerable<ExchangeRate>>
{
    private readonly string _exchangeRateCurrency;

    public ExchangeRateValueResolver(IConfiguration configuration)
    {
        _exchangeRateCurrency = configuration["ExchangeRateCurrency"] ?? throw new ArgumentNullException(nameof(configuration));
    }

    public IEnumerable<ExchangeRate> Convert(ExchangeRatesResponse source, IEnumerable<ExchangeRate> destMember, ResolutionContext? context)
    {
        return source.ExchangeRates.Select(exchangeRateResponse =>
        {
            if (string.IsNullOrWhiteSpace(exchangeRateResponse.Code))
            {
                throw new ArgumentException(nameof(exchangeRateResponse.Code));
            }
            
            return new ExchangeRate(
                new Currency(exchangeRateResponse.Code), 
                new Currency(_exchangeRateCurrency), 
                Math.Round(exchangeRateResponse.Rate / exchangeRateResponse.Amount, 3));
        }).ToList();
    }
}