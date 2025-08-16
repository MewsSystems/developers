using CNB.Client;
using ExchangeRates.App.Caching;
using ExchangeRates.Domain;
using NodaTime;
using NodaTime.Extensions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRates.App.Provider;

/// <summary>
/// Provides exchange rates for the given currencies against the base currency (CZK).
/// </summary>
public class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly IBankClient _client;
    private readonly ICacheService _cache;
    private readonly Currency _baseCurrency = new("CZK");
    private readonly IClock _clock;

    public ExchangeRateProvider(IBankClient client, ICacheService cache, IClock clock)
    {
        _client = client;
        _cache = cache;
        _clock = clock;
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(
        IEnumerable<Currency> currencies, CancellationToken cancellationToken)
    {
        List<ExchangeRate> list = new();
        var today = _clock.InZone(DateTimeZoneProviders.Tzdb["Europe/Prague"]).GetCurrentDate().ToDateOnly();
        foreach (var currency in currencies)
        {
            var fromCache =
                _cache.GetCachedData<ExchangeRate>(new string($"{currency.Code}/{_baseCurrency.Code}"));
            if (fromCache != null)
            {
                list.Add(new ExchangeRate(fromCache.SourceCurrency, _baseCurrency,
                    fromCache.Value));
                continue;
            }

            //missed cache, let's get it from the source.
            var rates = await _client.GetRatesDaily(today, cancellationToken);
            //TODO: we can update only the requested currency, not all of them, but for the sake of simplicity, let's keep it this way
            rates.ForEach(exchangeRate => _cache.SetCachedData(new string($"{exchangeRate.SourceCurrency.Code}/{_baseCurrency.Code}"), exchangeRate));

            //let's try to get it from cache again
            var fromCache2 =
                _cache.GetCachedData<ExchangeRate>(new string($"{currency.Code}/{_baseCurrency.Code}"));

            //if it's still not there, then this currency is not supported.
            //TODO: this would be not necessary if we know for sure which currencies are supported by us.
            //E.g. we can have a list of supported currencies in the configuration or populate it based on the CNB Client response, if we assume that their data is the source of truth.
            if (fromCache2 == null)
            {
                continue;
            }

            list.Add(new ExchangeRate(fromCache2.SourceCurrency, _baseCurrency,
                fromCache2.Value));
        }

        return list;
    }
}
