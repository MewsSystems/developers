using ExchangeRateProviderAPI_PaolaRojas.Models;
using ExchangeRateProviderAPI_PaolaRojas.Models.Options;
using ExchangeRateProviderAPI_PaolaRojas.Models.Responses;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace ExchangeRateProviderAPI_PaolaRojas.Services
{
    public class ExchangeRateService(
        IOptions<CnbOptions> options,
        IMemoryCache cache,
        ILogger<ExchangeRateService> logger,
        HttpClient httpClient) : IExchangeRateService
    {
        private readonly string _cnbBaseUrl = options.Value.DailyExchangeBaseUrl;
        private readonly IMemoryCache _cache = cache;
        private readonly ILogger<ExchangeRateService> _logger = logger;
        private readonly HttpClient _httpClient = httpClient;

        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

        public async Task<ExchangeRateResponse> GetExchangeRatesAsync(IEnumerable<Currency> requestedCurrencies)
        {
            var requestedCodes = requestedCurrencies
                .Select(c => c.Code.ToUpperInvariant())
                .ToHashSet();

            var today = DateTime.UtcNow.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            var cacheKey = $"CNB_ExchangeRates_{DateTime.UtcNow:yyyyMMdd}";

            List<ExchangeRate> allRates;

            try
            {
                allRates = await _cache.GetOrCreateAsync(cacheKey, async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = CacheDuration;

                    string requestUrl = $"{_cnbBaseUrl}?date={today}";

                    var response = await _httpClient.GetStringAsync(requestUrl);

                    var lines = response.Split('\n');
                    var rates = new List<ExchangeRate>();

                    foreach (var line in lines.Skip(2))
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        var parts = line.Split('|');
                        if (parts.Length != 5) continue;

                        if (!int.TryParse(parts[2], out int amount)) continue;
                        string code = parts[3];
                        if (!decimal.TryParse(parts[4], NumberStyles.Any, CultureInfo.InvariantCulture, out var value)) continue;

                        var normalizedRate = value / amount;
                        var source = new Currency(code);
                        var target = new Currency("CZK");

                        rates.Add(new ExchangeRate(source, target, normalizedRate));
                    }

                    return rates;
                }) ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve or parse CNB exchange rate text data.");
                return new ExchangeRateResponse { ExchangeRates = [] };
            }

            var filtered = allRates
                .Where(r =>
                    requestedCodes.Contains(r.SourceCurrency.Code))
                .ToList();

            return new ExchangeRateResponse
            {
                ExchangeRates = filtered
            };
        }
    }
}