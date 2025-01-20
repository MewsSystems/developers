using ExchangeRateUpdater.Data.Interfaces;
using ExchangeRateUpdater.Data.Responses;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace ExchangeRateUpdater.Data.Repositories;
public class ExchangeRateCacheRepository : IExchangeRateCacheRepository
{   
    private readonly IMemoryCache _cache;
    private readonly ILogger<ExchangeRateCacheRepository> _logger;

    public ExchangeRateCacheRepository(IMemoryCache cache, ILogger<ExchangeRateCacheRepository> logger)
    {        
        _cache = cache;
        _logger = logger;
    }

    public ExchangeRatesResponseDto GetExchangeRates(DateTime date)
    {
        var result = new ExchangeRatesResponseDto() { Rates = new List<ExchangeRateDto>()};
        var key = date.ToString("yyyy-MM-dd");

        if (_cache.TryGetValue(key, out ExchangeRatesResponseDto ratesCached))
        {
            _logger.LogInformation($"{ratesCached.Rates.Count} retrieved from cache");
            result.Rates = ratesCached.Rates.Where(t => t.ValidFor == date).ToList();
        }             

        return result;
    }

    public void SetExchangeRates(ExchangeRatesResponseDto rates)
    {
        try
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromDays(1)); //Keep cache for 1 day max

            //Since weekends there's no rates and the api always returns the las data avaiable,
            //the rates in cache have the actual date and not the requested
            var key = rates.Rates[0].ValidFor.ToString("yyyy-MM-dd");

            if (!_cache.TryGetValue(key, out _))
            {
                _logger.LogInformation($"{rates.Rates.Count} have been saved on cache");
                _cache.Set(key, rates, cacheEntryOptions);
            }
                
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new Exception($"There was an error while saving in cache. Error: {ex.Message}");
        }        
    }
}
    
