using ExchangeRateUpdater.Services.Interfaces;
using ExchangeRateUpdater.Services.Models;
using ExchangeRateUpdater.Services.Models.External;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateProvider(
            IApiClient<CnbRate> apiClient,
            IExchangeRateCacheService cacheService,
            ILogger<ExchangeRateProvider> logger)
    {
        private const string TargetCurrencyCode = "CZK";

        /// <summary>
        /// Returns exchange rates among the specified currencies that are defined by the source.
        /// Omits the currencies if they are not returned in the external API response.
        /// </summary>
        public async Task<ICollection<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            ArgumentNullException.ThrowIfNull(currencies);

            var currencyCodes = currencies
                .Select(c => c.Code)
                .Where(code =>
                    !string.IsNullOrWhiteSpace(code) &&
                    !string.Equals(code, TargetCurrencyCode, StringComparison.OrdinalIgnoreCase))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            if (currencyCodes.Count == 0)
                return [];

            var cachedRates = cacheService.GetCachedRates(currencyCodes);
            var cachedCodes = cachedRates.Select(r => r.SourceCurrency.Code);

            // Check if any of the missing currency codes have already been cached as invalid. 
            // There's no need to call the API if all the remain missing codes are simply invalid.
            var invalidCodes = cacheService.GetInvalidCodes();
            var missingCodes = currencyCodes.Except(cachedCodes).Except(invalidCodes).ToHashSet();

            if (missingCodes.Count == 0)
                return cachedRates;

            var apiResponse = await apiClient.GetExchangeRatesAsync();
            var exchangeRates = FilterExchangeRates(apiResponse, currencyCodes);

            if (exchangeRates.Count < currencyCodes.Count)
            {
                var codesNotFound = currencyCodes.Except(exchangeRates.Select(r => r.SourceCurrency.Code));
                cacheService.UpdateInvalidCodes(codesNotFound);
                logger.LogWarning("Unable to find rates for the following currencies: [{CodesNotFound}]", string.Join(", ", codesNotFound));
            }

            return cachedRates.Concat(exchangeRates).ToList();
        }

        private static HashSet<ExchangeRate> FilterExchangeRates(IEnumerable<CnbRate> rates, HashSet<string> currencyCodes)
        {
            if (rates is null || currencyCodes.Count == 0)
                return [];

            // Filter rates with matching currency codes to a new collection.
            // To ensure consistent output, normalise currency rates returned
            // by the api so it's always per 1 unit.
            return rates
                .Where(rate =>
                        currencyCodes.Contains(rate.CurrencyCode))
                .Select(rate =>
                        new ExchangeRate(
                            new Currency(rate.CurrencyCode),
                            new Currency(TargetCurrencyCode),
                            rate.Amount == 1 ? rate.Rate : rate.Rate / rate.Amount))
                .ToHashSet();
        }
    }
}
