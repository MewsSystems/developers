using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public sealed class ExchangeRateProvider
    {
        private readonly IExchangeRateSource exchangeRateSource;

        public ExchangeRateProvider() : this(new Cnb.ExchangeRateWebSource())
        {
        }

        public ExchangeRateProvider(IExchangeRateSource exchangeRateSource)
        {
            this.exchangeRateSource = exchangeRateSource;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var availableExchangeRates = await GetAvailableRates();

            var resultRates = new List<ExchangeRate>();
            foreach (var currency in currencies)
            {
                if (availableExchangeRates.TryGetValue(currency.Code, out var rate))
                {
                    resultRates.Add(rate);
                }
            }

            return resultRates;
        }

        private async Task<Dictionary<string, ExchangeRate>> GetAvailableRates()
        {
            var rates = await this.exchangeRateSource.Load();

            var availableExchangeRates = new Dictionary<string, ExchangeRate>();
            foreach (var rate in rates)
            {
                if (!availableExchangeRates.ContainsKey(rate.SourceCurrency.Code))
                {
                    availableExchangeRates.Add(rate.SourceCurrency.Code, rate);
                }
                else
                {
                    throw new ExchangeRateProviderException($"Duplicate exchange rate found for target currency {rate.SourceCurrency}");
                }
            }

            return availableExchangeRates;
        }
    }
}
