using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const string RateUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        private const string StandardCurrencyCode = "CZK";
        private const char Pipe = '|';

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var exchangeRateList = new List<ExchangeRate>();

                    string txtResponse = httpClient.GetStringAsync(RateUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                    string[] txtLines = txtResponse.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    foreach (string line in txtLines)
                    {
                        var lineValues = line.Split(Pipe);

                        if (lineValues.Count() > 1)
                        {
                            var sourceCurrency = currencies.SingleOrDefault(q => q.Code == lineValues[3]);

                            if (sourceCurrency != null)
                            {
                                exchangeRateList.Add(new ExchangeRate(sourceCurrency, new Currency(StandardCurrencyCode), decimal.Parse(lineValues[4])));
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }

                    return exchangeRateList;
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
