using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ExchangeRateUpdater.DataAccess;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly ExchangeRateRepository _exchangeRateRepository;
        public ExchangeRateProvider(ExchangeRateRepository exchangeRateRepository)
        {
            _exchangeRateRepository = exchangeRateRepository;
        }
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var rates = await _exchangeRateRepository.GetRatesAsync();
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();
            
            foreach (var currency in currencies)
            {
                if (rates.ContainsKey(currency.Code))
                {
                    var rate = rates[currency.Code];
                    exchangeRates.Add(new ExchangeRate(new Currency(currency.Code), new Currency("CZK"), rate.Rate));
                } 
            }

            return exchangeRates;
        }
    }
}
