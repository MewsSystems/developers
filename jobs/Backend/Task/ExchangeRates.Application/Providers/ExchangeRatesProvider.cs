using ExchangeRates.Application.Options;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Infrastructure.Cache;
using ExchangeRates.Infrastructure.Clients.CNB;
using ExchangesRates.Infrastructure.External.CNB.Dtos;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ExchangeRates.Application.Providers
{
    public interface IExchangeRatesProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<string> currencyCodes, CancellationToken cancellationToken = default);
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken);
    }

    public class ExchangeRatesProvider : IExchangeRatesProvider
    {
        private readonly ICnbHttpClient _cnbHttpClient;
        private readonly IDistributedCache _cache;
        private readonly TimeOnly _refreshTimeCZ;
        private readonly string[] _defaultCurrencies;
        private readonly ILogger<ExchangeRatesProvider> _logger;

        public ExchangeRatesProvider(
            ICnbHttpClient cnbHttpClient,
            IDistributedCache cache,
            IOptions<CnbHttpClientOptions> cnbSettings,
            IOptions<ExchangeRatesOptions> exchangeRatesSettings,
            ILogger<ExchangeRatesProvider> logger)
        {
            _cnbHttpClient = cnbHttpClient;
            _cache = cache;
            _logger = logger;

            _refreshTimeCZ = cnbSettings.Value.DailyRefreshTimeCZ;
            _defaultCurrencies = exchangeRatesSettings.Value.DefaultCurrencies;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<string>? currencyCodes, CancellationToken cancellationToken = default)
        {
            var currencies = (currencyCodes != null && currencyCodes.Any())
                ? currencyCodes.Select(c => new Currency(c.Trim().ToUpperInvariant()))
                : _defaultCurrencies.Select(c => new Currency(c.Trim().ToUpperInvariant()));

            return await GetExchangeRatesAsync(currencies, cancellationToken);
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken = default)
        {
            var cacheKey = CacheKeys.ExchangeRatesDaily();
            CnbExRatesResponse? response;

            _logger.LogInformation("Fetching exchange rates for currencies: {Currencies}", string.Join(", ", currencies.Select(c => c.Code)));

            try
            {
                var cachedData = await _cache.GetStringAsync(cacheKey, cancellationToken);

                if (!string.IsNullOrEmpty(cachedData))
                {
                    _logger.LogInformation("Cache hit for key '{CacheKey}'. Deserializing exchange rates.", cacheKey);
                    response = JsonSerializer.Deserialize<CnbExRatesResponse>(cachedData)!;
                }
                else
                {
                    response = await _cnbHttpClient.GetDailyExchangeRatesAsync(cancellationToken: cancellationToken);

                    if (response?.Rates == null || !response.Rates.Any())
                    {
                        _logger.LogError("CNB API returned no exchange rate data.");
                        return Enumerable.Empty<ExchangeRate>();
                    }

                    var serialized = JsonSerializer.Serialize(response);
                    var apiDataLastUpdated = response.Rates.Min(c => c.ValidFor);
                    var expiration = CacheExpirationHelper.GetCacheExpirationToNextCzTime(_refreshTimeCZ, apiDataLastUpdated);

                    _logger.LogInformation("Caching exchange rates under key '{CacheKey}' for {Expiration} seconds.", cacheKey, expiration.TotalSeconds);
                    await _cache.SetStringAsync(
                        cacheKey,
                        serialized,
                        new DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = expiration
                        },
                        cancellationToken);
                }

                var czk = new Currency("CZK");
                var currencySet = currencies.Select(c => c.Code).ToHashSet();

                var result = response.Rates
                    .Where(r => currencySet.Contains(r.CurrencyCode))
                    .Select(r =>
                    {
                        var target = currencies.First(c => c.Code == r.CurrencyCode);
                        return new ExchangeRate(czk, target, r.Rate / r.Amount);
                    })
                    .ToList();

                _logger.LogInformation("Returning {Count} exchange rates.", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching exchange rates.");
                throw;
            }
        }
    }
}
