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
            var rates = new List<ExchangeRate>();

            // if rates are not present in cache, ask web service for current rates
            if (!_rateStorage.TryGetValue(_configuration.Value.RATE_STORAGE_KEY,
                    out Dictionary<string, ExchangeRateResult> currentExchangeRates))
            {
                _logger.LogDebug("Cache miss for rate retrieval. Retrieving rates from 3rd party API.");
                var exchangeRates = await _client.GetDailyExchangeRates();

                if (exchangeRates == null || exchangeRates.Rates.Length == 0)
                {
                    _logger.LogWarning("No exchange rates retrieved from the API.");
                    return Array.Empty<ExchangeRate>();
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
            }

            return rates;
        }
    }
}