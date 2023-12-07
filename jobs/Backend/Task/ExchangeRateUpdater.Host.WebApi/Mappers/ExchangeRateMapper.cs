using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Host.WebApi.Dtos.Response;

namespace ExchangeRateUpdater.Host.WebApi.Mappers;

/// <summary>
/// Static class responsible for mapping to dto of exchange rate functionality.
/// </summary>
internal static class ExchangeRateMapper
{
    /// <summary>
    /// Maps an ExchangeRate class to ExchangeRateDto.
    /// </summary>
    /// <param name="exchangeRate">Instance of ExchangeRate class.</param>
    /// <returns>Returns an instance of ExchangeRateDto.</returns>
    internal static ExchangeRateDto ToDto(this ExchangeRate exchangeRate)
    {
        return new ExchangeRateDto
        {
            From         = exchangeRate.SourceCurrency,
            To           = exchangeRate.TargetCurrency,
            ExchangeRate = exchangeRate.CurrencyRate,
            ExchangeRateDate = exchangeRate.RateDate
        };
    }
}
