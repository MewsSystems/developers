using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const char _valueDelimiter = '|';
        private const string _targetCurrencyString = "CZK";
        private const string _decimalSeparator = ",";

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var textRates = GetRates();

            return DeserializeRates(textRates, currencies);
        }

        private IEnumerable<ExchangeRate> DeserializeRates(string txtRates, IEnumerable<Currency> currencies)
        {
            var rates = new List<ExchangeRate>();

            var targetCurrency = currencies.FirstOrDefault(x => x.Code == _targetCurrencyString);
            if(targetCurrency == null)
            {
                targetCurrency = new Currency(_targetCurrencyString);
            }

            var lines = txtRates.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if(lines.Count() < 3)
            {
                throw new FormatException("There are no rates to parse.");
            }

            var exchangeRateLines = lines.Skip(2);
            foreach (var line in exchangeRateLines)
            {
                var values = line.Split(_valueDelimiter);

                //if there are more vaues probably something has changed
                //Country|Currency|Amount|Code|Rate
                //Austrálie|dolar|1|AUD|15,724
                if (values.Count() != 5)
                {
                    throw new FormatException("The format has probably changed and cannot be parsed.");
                }

                var currencyString = values[3];

                var sourceCurrency = currencies.FirstOrDefault(x => x.Code == currencyString);
                if(sourceCurrency == null)
                {
                    continue;
                }

                var rate = values[4];

                var nfi = new NumberFormatInfo { CurrencyDecimalSeparator = _decimalSeparator };
                //for now skips the rate if it cant parse the number 
                decimal decimalRate = 0;
                if (decimal.TryParse(rate, NumberStyles.Any, nfi, out decimalRate))
                {
                    rates.Add(new ExchangeRate(sourceCurrency, targetCurrency, decimalRate));
                }
            }
            return rates;
        }

        private string GetRates()
        {
            var url = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";

            string response;

            using (var httpClient = new HttpClient())
            {
                response = httpClient.GetStringAsync(url).Result;  
            }

            if (string.IsNullOrWhiteSpace(response))
            {
                throw new HttpRequestException("Page is not reachable or there were no rates returned.");
            }

            return response;
        }
    }
}
