using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly CNBClient cNBClient;

        public ExchangeRateProvider()
        {
            this.cNBClient = new CNBClient();
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(
            IEnumerable<Currency> currencies
        )
        {
            var cnbRates = await this.cNBClient.GetCurrentExchangeRates();
            var result = new List<ExchangeRate>();

            foreach (var currency in currencies)
            {
                if (!cnbRates.ContainsKey(currency.Code))
                    continue;

                result.Add(
                    new ExchangeRate(
                        this.cNBClient.DefaultCurrency,
                        currency,
                        cnbRates[currency.Code]
                    )
                );
            }

            return result;
        }
    }
}
