using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        CultureInfo cultureInfo;

        public ExchangeRateProvider()
        {
            cultureInfo = new CultureInfo(Constants.EnUs);
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var exchangeRateList = new List<ExchangeRate>();

            string txtResponse = LoadTextData();
            string[] txtLines = txtResponse.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in txtLines)
            {
                var lineValues = line.Split(Constants.Pipe);

                if (lineValues.Count() > 1)
                {
                    var sourceCurrency = currencies.SingleOrDefault(q => q.Code == lineValues[3]);

                    if (sourceCurrency != null)
                    {
                        exchangeRateList.Add(new ExchangeRate(sourceCurrency,
                                                              new Currency(Constants.CzechCurrencyCode),
                                                              decimal.Parse(lineValues[4], cultureInfo) / decimal.Parse(lineValues[2])
                                                             ));
                    }
                }
                else
                {
                    continue;
                }
            }

            return exchangeRateList;
        }

        private string LoadTextData()
        {
            string response = string.Empty;

            using (var httpClient = new HttpClient())
            {
                try
                {
                    response = httpClient.GetStringAsync(string.Format(Constants.RateUrl, Constants.Url)).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch (HttpRequestException)
                {
                    // Falls back to Czech version
                    cultureInfo = new CultureInfo(Constants.CsCz);
                    response = httpClient.GetStringAsync(string.Format(Constants.RateUrlCzech, Constants.Url)).ConfigureAwait(false).GetAwaiter().GetResult();
                }
            }

            return response;
        }
    }
}
