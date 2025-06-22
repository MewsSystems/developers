using ExchangeRateModel;

namespace ExchangeRateService.Cache;

public interface IExchangeRateCache
{
    
    Task AddExchangeRate(ExchangeRate exchangeRate);
    
    Task AddExchangeRates(IEnumerable<ExchangeRate> exchangeRates);
    
    /// <summary>
    /// Tries to get the cached exchange rate value, default if it's not present
    /// </summary>
    /// <param name="exchangeRate">Specified exchange rate without its value</param>
    /// <returns></returns>
    Task<ExchangeRate?> TryGetExchangeRate(ExchangeRate exchangeRate);
    
}