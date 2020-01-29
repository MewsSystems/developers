using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private string exchangeRateServiceURL =
            "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

        private Currency targetCurrency = new Currency("CZK");

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies == null || !currencies.Any())
                throw new ArgumentNullException(nameof(currencies));

            return GetExchangeRates().Where(r => currencies.Any(s => s.Code == r.SourceCurrency.Code));
        }

        private List<ExchangeRate> GetExchangeRates()
        {
            var exchangeRates = new List<ExchangeRate>();

            string exchangeDetailsString = GetExchangeRateSource(exchangeRateServiceURL);

            string[] lines = exchangeDetailsString.Split('\n');

            for (int i = 2; i < lines.Length; i++)
            {
                var exchange = ParseCurrency(lines[i]);
                if (exchange != null)
                {
                    exchangeRates.Add(exchange);
                }
            }

            return exchangeRates;
        }

        private string GetExchangeRateSource(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetStringAsync(new Uri(url)).Result;

                return response;
            }
        }

        private ExchangeRate ParseCurrency(string context)
        {
            if (string.IsNullOrEmpty(context))
            {
                return null;
            }

            var currencyDetail = context.Split('|');

            if (currencyDetail.Length != 5)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var amount = decimal.Parse((currencyDetail[2]));
            // I am not sure about below division operation but I want to show unit value on the screen.
            // TODO : Some amount value is bigger than 1 therefore we need to find unit value. 
            var currencyRate = Decimal.Divide(decimal.Parse(currencyDetail[4]), amount);

            return new ExchangeRate(new Currency(currencyDetail[3]), targetCurrency, currencyRate);
        }
    }
}