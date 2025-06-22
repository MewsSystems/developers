using System.Collections.Generic;
using Microsoft.Extensions.Logging;

using ExchangeRateModel;

namespace ExchangeRateService.Cache;

public class InMemmoryERCache : IExchangeRateCache
{

    private readonly ILogger<InMemmoryERCache> _logger;
    /// <summary>
    /// Access like [exchange rate currencies][date of the value] = exchange rate value for the current day
    /// </summary>
    private Dictionary<string, Dictionary<string, ExchangeRate>> _cache;
    
    public InMemmoryERCache(ILogger<InMemmoryERCache> logger)
    {
        _logger = logger;
        _cache = new();
    }
    
    
    public Task AddExchangeRate(ExchangeRate exchangeRate)
    {
        if(!_cache.ContainsKey(exchangeRate.ExchangeRateName()))
            _cache[exchangeRate.ExchangeRateName()] = new Dictionary<string, ExchangeRate>();
        
        _cache[exchangeRate.ExchangeRateName()][exchangeRate.Date.ToString("yyyy-MM-dd")] = exchangeRate;
        return Task.CompletedTask;
    }

    public Task AddExchangeRates(IEnumerable<ExchangeRate> exchangeRates)
    {
        foreach(var exchangeRate in exchangeRates)
            AddExchangeRate(exchangeRate);
        return Task.CompletedTask;
    }

    public Task<ExchangeRate?> TryGetExchangeRate(ExchangeRate exchangeRate)
    {
        if(!_cache.TryGetValue(exchangeRate.ExchangeRateName(), out var currencyRates))
            return Task.FromResult<ExchangeRate?>(null);
        return Task.FromResult(currencyRates.GetValueOrDefault(exchangeRate.Date.ToString("yyyy-MM-dd")));
    }
}