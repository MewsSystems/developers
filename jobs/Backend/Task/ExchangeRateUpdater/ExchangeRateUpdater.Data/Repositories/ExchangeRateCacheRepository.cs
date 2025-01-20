using ExchangeRateUpdater.Data.Interfaces;
using ExchangeRateUpdater.Data.Responses;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;


namespace ExchangeRateUpdater.Data.Repositories;
public class ExchangeRateCacheRepository : IExchangeRateCacheRepository
{
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _cache;

    public ExchangeRateCacheRepository(IConfiguration configuration, IMemoryCache cache)
    {
        _configuration = configuration;
        _cache = cache;
    }

    public ExchangeRatesResponseDto GetExchangeRates(DateTime date)
    {
        var result = new ExchangeRatesResponseDto() { Rates = new List<ExchangeRateDto>()};

        if (_cache.TryGetValue(_configuration["CacheKey"], out ExchangeRatesResponseDto ratesCached))
             result.Rates = ratesCached.Rates.Where(t => t.ValidFor == date).ToList();            

        return result;
    }

    public void SetExchangeRates(ExchangeRatesResponseDto rates)
    {
        try
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
           .SetAbsoluteExpiration(TimeSpan.FromDays(1)); //Keep cache for 1 day max

            _cache.Set(_configuration["CacheKey"], rates, cacheEntryOptions);
        }
        catch (Exception ex)
        {
            throw new Exception($"There was an error while saving in cache. Error: {ex.Message}");
        }        
    }
}
    
