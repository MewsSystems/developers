using System.Net.Http.Json;
using ExchangeRateUpdater.Infrastructure.Cache;
using ExchangeRateUpdater.Infrastructure.CNB.Entities;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Infrastructure.CNB;

public interface ICnbProvider
{
    Task<CnbExchangeResponseEntity> GetLatestExchangeInformation();
}

public class CnbProvider(ICacheProvider cache, HttpClient cnbClient, IOptions<CnbConfiguration> cnbConfigurations)
    : ICnbProvider
{
    public async Task<CnbExchangeResponseEntity> GetLatestExchangeInformation()
    {
        if (cache.TryGetCache(cnbConfigurations.Value.CacheKeyBase,
                out CnbExchangeResponseEntity? cachedRates) && cachedRates is not null)
        {
            return cachedRates;
        }

        CnbExchangeResponseEntity exchangeRates = await GetExchangeRatesFromCzechNationalBank();

        _ = cache.TrySetCache(cnbConfigurations.Value.CacheKeyBase, exchangeRates,
            cnbConfigurations.Value.CacheTTLInSeconds);

        return exchangeRates;
    }


    private async Task<CnbExchangeResponseEntity> GetExchangeRatesFromCzechNationalBank()
    {
        CnbExchangeResponseEntity? cnbResponse = await cnbClient.GetFromJsonAsync<CnbExchangeResponseEntity>(
            $"cnbapi/exrates/daily?&lang={cnbConfigurations.Value.Language}");

        return cnbResponse!;
    }
}