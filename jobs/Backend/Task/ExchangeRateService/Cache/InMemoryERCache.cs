using System.Collections.Generic;
using Microsoft.Extensions.Logging;

using ExchangeRateModel;

namespace ExchangeRateService.Cache;

/// <summary>
/// Implements IExchangeRateCache with in memory management
/// </summary>
public class InMemoryERCache : IExchangeRateCache
{

    private readonly ILogger<InMemoryERCache> _logger;
    /// <summary>
    /// Access like [exchange rate currencies][date of the value] = exchange rate value for the current day
    /// </summary>
    private Dictionary<string, Dictionary<string, ExchangeRate>> _cache;
    
    public InMemoryERCache(ILogger<InMemoryERCache> logger)
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

    public Task<bool> TryGetExchangeRate(ExchangeRate exchangeRate, out ExchangeRate outExchangeRate)
    {
        outExchangeRate = default;
        _logger.LogDebug($"Getting exchange rate for {exchangeRate.ExchangeRateName()} from cache");
        if (!_cache.TryGetValue(exchangeRate.ExchangeRateName(), out var currencyRates))
        {
            _logger.LogDebug("Exchange rate not found");
            return Task.FromResult(false);
        }

        _logger.LogDebug($"Getting exchange rate for {exchangeRate.ExchangeRateName()} and {exchangeRate.Date:yyyy-MM-dd}");
        
        if (!currencyRates.TryGetValue(exchangeRate.Date.ToString("yyyy-MM-dd"), out var currencyRate))
        {
            _logger.LogDebug("Cache miss");
            return Task.FromResult(false);
        }
        _logger.LogDebug("Cache hit!");
        outExchangeRate = currencyRate;
        return Task.FromResult(true);
    }

    public async Task<IList<ExchangeRate>> GetExchangeRates(IList<ExchangeRate> exchangeRates)
    {
        _logger.LogDebug("Getting exchange rates from cache");
        var result = new List<ExchangeRate>();
        
        foreach (var rate in exchangeRates)
        {
            if(await TryGetExchangeRate(rate, out var exchangeRate))
                result.Add(exchangeRate);
        }
        _logger.LogDebug("Returning exchange rates from cache");
        return result;
    }
}