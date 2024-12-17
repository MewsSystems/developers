using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.ApplicationServices.Interfaces;

/// <summary>
/// Czech National Bank exchange rate repository interface.
/// </summary>
/// <seealso cref="ExchangeRateUpdater.ApplicationServices.Interfaces.IExchangeRateRepository" />
public interface IExchangeRateRepository
{
    /// <summary>
    /// Gets today's exchange rates asynchronous.
    /// </summary>
    /// <returns>An Enumeration of <see cref="ExchangeRateDto"/></returns>
    Task<IEnumerable<ExchangeRate>> GetTodayExchangeRatesAsync();

    /// <summary>
    /// Gets the exchange rates asynchronously for a given date.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns>An Enumeration of <see cref="ExchangeRateDto"/></returns>
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(DateTime date);
}
