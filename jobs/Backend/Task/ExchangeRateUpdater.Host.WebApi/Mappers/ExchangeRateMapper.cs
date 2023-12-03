using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Host.WebApi.Dtos.Response;

namespace ExchangeRateUpdater.Host.WebApi.Mappers;

internal static class ExchangeRateMapper
{
    internal static ExchangeRateDto ToDto(this ExchangeRate exchangeRate)
    {
        return new ExchangeRateDto
        {
            From         = exchangeRate.SourceCurrency,
            To           = exchangeRate.TargetCurrency,
            ExchangeRate = exchangeRate.CurrencyRate 
        };
    }
}
