using ExchangeRateUpdater.ApplicationServices.ExchangeRates.Dto;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.ApplicationServices.ExchangeRates;

/// <summary>
/// Exchange rate application service interface.
/// The service layer intermediates between internal model and exposed API model.
/// 
/// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
/// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
/// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
/// some of the currencies, ignore them.
/// </summary>
public interface IExchangeRateService
{
    /// <summary>
    /// Gets the exchange rates asynchronously.
    /// </summary>
    /// <param name="currencies">The currencies to check.</param>
    /// <param name="date">The date to check.</param>
    /// <returns>An Enumeration of <see cref="ExchangeRateDto"/></returns>
    Task<IEnumerable<ExchangeRateDto>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, DateTime date);

    /// <summary>
    /// Gets the exchange rates asynchronously for a given date.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns>An Enumeration of <see cref="ExchangeRateDto"/></returns>
    Task<IEnumerable<ExchangeRateDto>> GetExchangeRatesAsync(DateTime date);

    /// <summary>
    /// Gets today's exchange rates asynchronously.
    /// </summary>
    /// <param name="currencies">The currencies to check (optional)</param>
    /// <returns>
    /// An Enumeration of <see cref="ExchangeRateDto" />
    /// </returns>
    Task<IEnumerable<ExchangeRateDto>> GetTodayExchangeRatesAsync(IEnumerable<Currency>? currencies = null);
}
