using System;
using System.Collections.Generic;
using System.Net;

namespace ExchangeRateProvider
{
    public class ExchangeRateProvider
    {
        private IExchangeRateFileHandler fileHandler;

        public ExchangeRateProvider(IExchangeRateFileHandler fileHandler)
        {
            this.fileHandler = fileHandler;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<CurrencyCode> currencies, string websiteUrl)
        {
            IDictionary<CurrencyCode, ExchangeRate> rates;
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();

            if (fileHandler.IsCachedFileUpToDate())
            {
                // get rates from cached file
                rates = fileHandler.Read();
            }
            else
            {
                CurrencyCode targetCurrency = new CurrencyCode("CZK");
                // get rates from web site
                rates = GetRatesFromWebsite(websiteUrl, targetCurrency);
                // cache retrieved exchange rates
                fileHandler.Write(rates);
            }

            foreach(CurrencyCode currency in currencies)
            {
                if (rates.TryGetValue(currency, out ExchangeRate rate))
                {
                    exchangeRates.Add(rate);
                }
            }

            return exchangeRates;
        }

        /// <summary>
        /// Downloads exchange rates from specified URL.
        /// </summary>
        /// <param name="websiteUrl">URL that contains exchange rates.</param>
        /// <param name="targetCurrency">Target currency for which rates apply.</param>
        /// <returns><see cref="IDictionary{TKey, TValue}"/> where TKey is a <see cref="CurrencyCode"/>
        /// and TValue is a <see cref="ExchangeRate"/></returns>
        private IDictionary<CurrencyCode, ExchangeRate> GetRatesFromWebsite(string websiteUrl, CurrencyCode targetCurrency)
        {
            var exchangeRates = new Dictionary<CurrencyCode, ExchangeRate>();
            using (WebClient webClient = new WebClient())
            {
                var exchangeRatesFile = webClient.DownloadString(websiteUrl);

                if (string.IsNullOrEmpty(exchangeRatesFile))
                {
                    throw new Exception("Requested URL is empty!");
                }

                // break downloaded string by lines
                string[] lines = exchangeRatesFile.Split('\n');

                // starting from 2 will omit first two lines as that is not important to us
                for(int i = 2; i<lines.Length - 1; i++)
                {
                    string[] line = lines[i].Split('|');
                    int amount = int.Parse(line[2]);
                    CurrencyCode sourceCurrency = new CurrencyCode(line[3]);
                    CurrencyCode targetCurrenty = targetCurrency;
                    decimal rate = decimal.Parse(line[4]);
                    ExchangeRate exchangeRate = new ExchangeRate(amount, sourceCurrency, targetCurrenty, rate);
                    exchangeRates.Add(sourceCurrency, exchangeRate);
                }
            }

            return exchangeRates;
        }
    }
}
