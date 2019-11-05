using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly string _baseUrl =
           "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var exchangeData = string.Empty;
            var webClient = new WebClient();
            try
            {
                exchangeData = webClient.DownloadString(_baseUrl);
            }
            catch (WebException)
            {
                //do something with exception
            }
            finally
            {
                webClient.Dispose();
            }

            if (string.IsNullOrEmpty(exchangeData))
            {
                return null;
            }

            return currencies
                    .Where(currency => currency.Code != "CZK")
                    .Select(sourceCurrency =>
                    {
                        var value = GetExchangeRateValue(sourceCurrency, exchangeData);
                        return value.HasValue ?
                            new ExchangeRate(
                            sourceCurrency: sourceCurrency,
                            targetCurrency: new Currency("CZK"), //the only target currency wich is available is CZK
                            value: value.Value) : null;
                    }).Where(exchangeRate => exchangeRate != null);
        }

        private decimal? GetExchangeRateValue(Currency sourceCurrency, string exchangeData)
        {
            var currencyDataRegex = new Regex($"\\d+\\|{sourceCurrency.Code}\\|\\d+\\,\\d+");
            var currencyMatchResult = currencyDataRegex.Match(exchangeData).Value.Split('|');

            if (currencyMatchResult.Length != 3)
            {
                return null;
            }

            if (Decimal.TryParse(currencyMatchResult[2].Replace(',', '.'), out var value))
            {
                if (int.TryParse(currencyMatchResult[0], out var amount))
                {
                    return value / amount; //I suppose it is not in contradiction with task description 
                }
                return null;
            }
            return null;
        }
    }
}
