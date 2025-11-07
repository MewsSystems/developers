using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace ExchangeRateUpdater.Services;

public sealed class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly ICnbClient _client;
    private readonly ICnbParser _parser;
    private readonly IMemoryCache _cache; 

    private const string CacheKey = "CNB_DAILY_PARSED";

    public ExchangeRateProvider(ICnbClient client, ICnbParser parser, IMemoryCache cache)
    {
        _client = client;
        _parser = parser;
        _cache = cache;
    }

    public async Task<IReadOnlyList<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken ct)
    {
        var requested = currencies.Select(c => c.Code.ToUpperInvariant()).ToHashSet();

        var parsed = await GetParsedDailyAsync(ct);

        var result = new List<ExchangeRate>(requested.Count);

        foreach (var (code, amount, rate) in parsed)
        {
            if (!requested.Contains(code) || code == "CZK") continue; 
            if (amount <= 0) continue; 

            var perOne = decimal.Round(rate / amount, 6, System.MidpointRounding.AwayFromZero);
            result.Add(new ExchangeRate(new Currency(code), new Currency("CZK"), perOne));
        }

        return result;
    }

    private async Task<IReadOnlyList<(string Code, int Amount, decimal Rate)>> GetParsedDailyAsync(CancellationToken ct)
    {
        if (_cache.TryGetValue(CacheKey, out IReadOnlyList<(string, int, decimal)> cached))
            return cached;

        var payload = await _client.GetDailyRatesAsync(ct);
        var parsed = _parser.Parse(payload).ToList().AsReadOnly();

        _cache.Set(CacheKey, parsed, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = System.TimeSpan.FromHours(18)
        });

        return parsed;
    }
}
