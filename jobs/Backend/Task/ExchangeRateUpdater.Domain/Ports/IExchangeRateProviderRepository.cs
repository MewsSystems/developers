using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.ValueObjects;

namespace ExchangeRateUpdater.Domain.Ports;

public interface IExchangeRateProviderRepository
{
    /// <summary>
    /// Gets all the fx rates for the specified date or earlier.
    /// </summary>
    /// <param name="exchangeRateDate">Date time in format dd.MM.yyyy</param>
    /// <returns>Returns the list of all fx rate</returns>
    Task<IEnumerable<ExchangeRate>> GetAllFxRates(DateTime exchangeRateDate);

    /// <summary>
    /// Get the exchange rates for Source/Target currency from a date interval.
    /// </summary>
    /// <param name="sourceCurrency">The currency to convert from.</param>
    /// <param name="targetCurrency">The currency to convert to.</param>
    /// <param name="From">The beginning of the rates time interval</param>
    /// <param name="To">The end of the rates time interval</param>
    /// <returns>Returns the list of fx exchange rates found during the specified interval.</returns>
    /// <remarks>We consider the interval to be [From, To]</remarks>
    /// /// <summary>
    Task<IEnumerable<ExchangeRate>> GetExchangeRateForCurrenciesAsync(Currency sourceCurrency, Currency targetCurrency, DateTime From, DateTime To);
}