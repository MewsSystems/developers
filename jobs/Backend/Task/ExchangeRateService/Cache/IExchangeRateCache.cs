using ExchangeRateModel;

namespace ExchangeRateService.Cache;

/// <summary>
/// Represents a cache for storing exchange rates
/// </summary>
public interface IExchangeRateCache
{
    
    /// <summary>
    /// Adds an exchange rate to the cache
    /// </summary>
    /// <param name="exchangeRate"></param>
    /// <returns></returns>
    Task AddExchangeRate(ExchangeRate exchangeRate);
    
    /// <summary>
    /// Adds a list of exchange rates to the cache
    /// </summary>
    /// <param name="exchangeRates"></param>
    /// <returns></returns>
    Task AddExchangeRates(IEnumerable<ExchangeRate> exchangeRates);
    
    /// <summary>
    /// Tries to get the cached exchange rate value, default if it's not present
    /// </summary>
    /// <param name="exchangeRate">A specific exchange rate without its value</param>
    /// <returns></returns>
    Task<ExchangeRate?> TryGetExchangeRate(ExchangeRate exchangeRate);
    
    /// <summary>
    /// Tries to get the cached exchange rate values for specified exchanges
    /// </summary>
    /// <param name="exchangeRates">A list of specific exchange rates without its value</param>
    /// <returns></returns>
    Task<IList<ExchangeRate>> TryGetExchangeRates(IList<ExchangeRate> exchangeRates);
    
}