using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var sourceData = GetSourceData();

            return sourceData.Where(s => currencies.FirstOrDefault(c => c.Code == s.SourceCurrency.Code) != null);
        }

        private IEnumerable<ExchangeRate> GetSourceData()
        {
            string mainCurrenciesDataUrl = "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt";
            string otherCurrenciesDataUrl = "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/other_currencies_fx_rates/fx_rates.txt";

            var mainData = ConvertSourceString(GetSourceString(mainCurrenciesDataUrl));
            var otherData = ConvertSourceString(GetSourceString(otherCurrenciesDataUrl));

            return mainData.Concat(otherData);
        }

        private string GetSourceString(string sourceDataUrl)
        {
            return new WebClient().DownloadString(sourceDataUrl);
        }

        private IEnumerable<ExchangeRate> ConvertSourceString(string sourceString)
        {
            var result = new List<ExchangeRate>();
            var targetCurrency = "CZK";
            var beginDataIndex = 2;
            var rateIndex = 4;
            var currencyCodeIndex = 3;

            var ratesStrings = sourceString.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Skip(beginDataIndex).ToArray();

            foreach (var rateString in ratesStrings)
            {
                var sourceArray = rateString.Split('|');

                if (decimal.TryParse(sourceArray[rateIndex], out decimal parsedRate))
                {
                    result.Add(new ExchangeRate(new Currency(sourceArray[currencyCodeIndex]), new Currency(targetCurrency), parsedRate));
                }
            }

            return result;
        }
    }
}
