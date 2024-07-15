using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Model;

namespace ExchangeRateUpdater
{
    
    public class ExchangeRateProvider
    {
        private readonly ICurrencyRateClient _currencyRateClient;
        public ExchangeRateProvider(ICurrencyRateClient currencyRateClient)
        {
            _currencyRateClient = currencyRateClient;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var fromDateTime = DateOnly.FromDateTime(DateTime.Now);
            var existingRates = DbMock.FindExchangeRates(fromDateTime);
            var allRates = existingRates ?? FetchAndStoreNewRates(fromDateTime);
            // Filter for the queried ones only
            return allRates.FindAll(rate => currencies.Contains(rate.SourceCurrency));
        }

        private List<ExchangeRate> FetchAndStoreNewRates(DateOnly forDate)
        {
            Console.WriteLine($"Fetching new exchange rates for {forDate}");
            var newRates = _currencyRateClient.GetExchangeRates(forDate).Result;
            Console.WriteLine($"Received {newRates.Count} new rates from {_currencyRateClient.GetType().Name}");
            DbMock.Insert(forDate, newRates);
            return newRates;
        }
    }
}
