using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.ExternalVendors.CzechNationalBank
{
    public class CzechNationalBankExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ILogger<CzechNationalBankExchangeRateProvider> _logger;
        private readonly IExchangeRateClient _client;
        private readonly IMemoryCache _rateStorage;

        public CzechNationalBankExchangeRateProvider(ILogger<CzechNationalBankExchangeRateProvider> logger, IExchangeRateClient client, IMemoryCache rateStorage)
        {
            _logger = logger;
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
            var RateStorage = new ConcurrentDictionary<string, ExchangeRateResult>();

            var exchangeRates = await _client.GetDailyExchangeRates();

            foreach (ExchangeRateResult rate in exchangeRates.Rates)
            {
                RateStorage.AddOrUpdate(rate.CurrencyCode, rate, (s, rate1) => rate);
            }

            foreach (Currency currency in currencies)
            {
                if (RateStorage.TryGetValue(currency.Code, out var requestedCurrencyRate))
                {
                    rates.Add(new ExchangeRate(currency, 
                        new Currency("CZK"), 
                        requestedCurrencyRate.Rate / requestedCurrencyRate.Amount // divide Rate / Amount to ensure all rates returned are consistent with 1 CZK
                        ) 
                    );
                }
            }

            return rates;
        }
    }
}
