using Mews.ExchangeRateUpdater.Application.ExchangeRates.Dto;
using Mews.ExchangeRateUpdater.Domain.Entities.ExchangeRateAgg;

namespace Mews.ExchangeRateUpdater.Application.ExchangeRates;

/// <summary>
/// Exchange rates application service definition.
/// </summary>
public interface IExchangeRateAppService
{
    /// <summary>
    /// Gets a collection of today's exchange rates.
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    /// <param name="currencies">Currencies to check</param>
    /// <returns>List of <see cref="ExchangeRateDto"/></returns>
    Task<IEnumerable<ExchangeRateDto>> GetTodayExchangeRatesAsync(List<Currency>? currencies = null);
}
