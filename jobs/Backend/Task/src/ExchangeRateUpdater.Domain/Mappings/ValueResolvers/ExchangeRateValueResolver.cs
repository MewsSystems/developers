using AutoMapper;
using ExchangeRateUpdater.Clients.Cnb.Responses;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Options;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Domain.Mappings.ValueResolvers;

public class ExchangeRateValueResolver : ITypeConverter<ExchangeRatesResponse, IEnumerable<ExchangeRate>>
{
    private readonly ApplicationOptions _applicationOptions;

    public ExchangeRateValueResolver(IOptions<ApplicationOptions> applicationOptions)
    {
        _applicationOptions = applicationOptions.Value ?? throw new ArgumentNullException(nameof(applicationOptions));
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
                new Currency(_applicationOptions.ExchangeRateCurrency), 
                Math.Round(exchangeRateResponse.Rate / exchangeRateResponse.Amount, 3));
        }).ToList();
    }
}