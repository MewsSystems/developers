using Mews.ExchangeRateMonitor.Common.Domain.Results;
using Mews.ExchangeRateMonitor.ExchangeRate.Domain;
using Mews.ExchangeRateMonitor.ExchangeRate.Features.Features.GetExratesDaily;
using Mews.ExchangeRateMonitor.ExchangeRate.Features.Options;
using Mews.ExchangeRateMonitor.ExchangeRate.Features.Shared.HttpClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Mews.ExchangeRateMonitor.ExchangeRate.Features.Features;

public interface IExchangeRateProvider
{
    Task<Result<IEnumerable<CurrencyExchangeRate>>> GetDailyRatesAsync(DateOnly date, CancellationToken ct);
}

public sealed class ExchangeRateProvider(
    ILogger<ExchangeRateProvider> logger,
    IOptions<ExchangeRateModuleOptions> opts,
    IMemoryCache cache,
    IHttpClientFactory httpClientFactory) : IExchangeRateProvider
{
    private const string TargetCurrencyCode = "CZK";
    private static string CnbDailyKey(DateOnly d) => $"cnb:daily:{d:yyyy-MM-dd}";

    public async Task<Result<IEnumerable<CurrencyExchangeRate>>> GetDailyRatesAsync(DateOnly date, CancellationToken ct)
    {
        var key = CnbDailyKey(date);
        if (cache.TryGetValue<List<CurrencyExchangeRate>>(key, out var cached) && cached is not null)
            return cached;

        var client = httpClientFactory.CreateClient(HttpClientConsts.HttpCnbClient);
        var url = $"exrates/daily?date={date:yyyy-MM-dd}";

        try
        {
            var cnbDailyRates = await client.GetFromJsonAsync<CnbApiDailyRatesResponseDto>(url, ct);
            if (cnbDailyRates?.Rates is null)
            {
                var msg = $"CNB response was null or missing 'rates' for ${url}";
                logger.LogWarning(msg);
                return Error.Failure($"{nameof(GetDailyRatesAsync)}", msg);
            }

            var reqCurr = opts.Value.CnbExratesOptions.RequiredCurrencies.ToHashSet();
            var filteredRates = cnbDailyRates.Rates
                .Where(cnbRate => reqCurr.Any(c => c == cnbRate.CurrencyCode))
                .Select(x => x.ToCurrencyExchangeRate(TargetCurrencyCode))
                .ToList();

            cache.Set(key, filteredRates, BuildCacheOptions(date, key));

            return filteredRates;
        }
        catch (HttpRequestException ex)
        {
            var msg = $"HTTP error calling CNB for {url}";
            logger.LogError(ex, msg);
            return Error.Failure(nameof(GetDailyRatesAsync), msg);
        }
        catch (Exception ex)
        {
            var msg = $"Unexpected error calling CNB for {url}";
            logger.LogError(ex, msg);
            return Error.Failure(nameof(GetDailyRatesAsync), msg);
        }
    }


    private MemoryCacheEntryOptions BuildCacheOptions(DateOnly date, string key) =>
    new MemoryCacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = date < DateOnly.FromDateTime(DateTime.UtcNow)
            ? TimeSpan.FromDays(30)   // historical = long TTL
            : TimeSpan.FromMinutes(10) // today = short TTL
    }
    .SetSize(1)
    .RegisterPostEvictionCallback((_, _, reason, _) =>
        logger.LogDebug($"Cache evicted {key} due to {reason}"));
}