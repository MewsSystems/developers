using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.ExternalVendors.CzechNationalBank
{
    public class CzechNationalBankExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ILogger<CzechNationalBankExchangeRateProvider> _logger;
        private readonly IOptions<CzechNationalBankSettings> _configuration;
        private readonly IExchangeRateClient _client;
        private readonly IMemoryCache _rateStorage;

        public CzechNationalBankExchangeRateProvider(ILogger<CzechNationalBankExchangeRateProvider> logger,
            IExchangeRateClient client, IMemoryCache rateStorage, IOptions<CzechNationalBankSettings> configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _client = client;
            _rateStorage = rateStorage;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            _logger.LogInformation("Retrieving exchange rates for { currencies }", currencies);
            var currentExchangeRates = await GetExchangeRatesOrFetchFromCache();
            return currentExchangeRates == null
                ? Enumerable.Empty<ExchangeRate>()
                : FormatRates(currencies, currentExchangeRates);
        }

        /// <summary>
        /// Retrieves exchange rates from the cache if available, otherwise fetches them from a web service and stores them in the cache.
        /// </summary>
        /// <returns>A dictionary containing the exchange rates, with currency codes as keys and exchange rate results as values.</returns>
        private async Task<Dictionary<string, ExchangeRateResult>> GetExchangeRatesOrFetchFromCache()
        {
            // if rates are not present in cache, ask web service for current rates
            if (!_rateStorage.TryGetValue(_configuration.Value.RATE_STORAGE_KEY,
                    out Dictionary<string, ExchangeRateResult> currentExchangeRates))
            {
                _logger.LogDebug("Cache miss for rate retrieval. Retrieving rates from 3rd party API.");
                var exchangeRates = await _client.GetDailyExchangeRates();

                if (exchangeRates == null || exchangeRates.Rates.Count == 0)
                {
                    _logger.LogWarning("No exchange rates retrieved from the API.");
                    return new Dictionary<string, ExchangeRateResult>();
                }

                Dictionary<string, ExchangeRateResult> rateDictionary =
                    exchangeRates.Rates.ToDictionary(rate => rate.CurrencyCode);

                // TODO: Went with a naive expiration time, ideally this would be based off of whenever the external service refreshes their results. 
                _rateStorage.Set(_configuration.Value.RATE_STORAGE_KEY, rateDictionary,
                    TimeSpan.FromMinutes(_configuration.Value.REFRESH_RATE_IN_MINUTES));
                currentExchangeRates = rateDictionary;
            }
            else
            {
                _logger.LogDebug("Cache hit for rate retrieval. Retrieving exchange rates from cache.");
            }

            return currentExchangeRates;
        }

        /// <summary>
        /// Formats the exchange rates for the specified currencies based on the current exchange rate values.
        /// </summary>
        /// <param name="currencies">The list of currencies to format the exchange rates for.</param>
        /// <param name="currentExchangeRates">The dictionary of current exchange rates.</param>
        /// <returns>The formatted exchange rates.</returns>
        private IEnumerable<ExchangeRate> FormatRates(IEnumerable<Currency> currencies,
            Dictionary<string, ExchangeRateResult> currentExchangeRates)
        {
            List<ExchangeRate> rates = new List<ExchangeRate>();

            foreach (Currency currency in currencies)
            {
                if (currentExchangeRates.TryGetValue(currency.Code, out var requestedCurrencyRate))
                {
                    rates.Add(new ExchangeRate(currency,
                            new Currency("CZK"),
                            requestedCurrencyRate.Rate /
                            requestedCurrencyRate
                                .Amount // divide Rate / Amount to ensure all rates returned are consistent with 1 CZK
                        )
                    );
                }
                else
                {
                    _logger.LogWarning(
                        $"Desired currency {currency.Code} was not found in Exchange Rate table. Ignoring.");
                }
            }

            return rates;
        }
    }
}