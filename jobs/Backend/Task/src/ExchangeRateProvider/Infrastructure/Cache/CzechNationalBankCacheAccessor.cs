using ExchangeRateUpdater.Infrastructure.Cache.Abstract;
using ExchangeRateUpdater.Infrastructure.Clients.CzechNationalBank;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace ExchangeRateUpdater.Infrastructure.Cache;

public interface ICzechNationalBankCacheAccessor : ICacheAccessor<IEnumerable<CzechNationalBankExchangeRate>>
{
}

public class CzechNationalBankCacheAccessor(ILogger<CzechNationalBankCacheAccessor> logger, IDistributedCache distributedCache) : ICzechNationalBankCacheAccessor
{
    private const string _allCnbExRatesKey = "AllCnbExRates";

    private readonly ILogger<CzechNationalBankCacheAccessor> _logger = logger;
    private readonly IDistributedCache _distributedCache = distributedCache;

    public async Task<IEnumerable<CzechNationalBankExchangeRate>?> GetAsync()
    {
        try
        {
            var cachedExRates = await _distributedCache.GetStringAsync(_allCnbExRatesKey);
            if (cachedExRates is not null)
            {
                return JsonConvert.DeserializeObject<IEnumerable<CzechNationalBankExchangeRate>>(cachedExRates);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred when retrieving all Czech National Bank exchange rates from distributed cache");
        }

        return null;
    }

    public async Task SetAsync(IEnumerable<CzechNationalBankExchangeRate> exchangeRates)
    {
        try
        {
            _logger.LogInformation("Persisting Czech National Bank exchange rates into cache");
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1));
            await _distributedCache.SetStringAsync(_allCnbExRatesKey, JsonConvert.SerializeObject(exchangeRates), options);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred when persisting all Czech National Bank exchange rates in distributed cache");
        }
    }
}
