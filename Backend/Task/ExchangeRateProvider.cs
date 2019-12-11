using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private static readonly string[] DataSource =
        {
            "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt",
            "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt"
        };
        private CurrentRatesData _currentRates;

        /// <summary>
        ///     With new Provider, immediately download current rates
        /// </summary>
        public ExchangeRateProvider()
        {
            LoadCurrentRates();
        }

        /// <summary>
        ///     Will load data from Sources and stored them into SortedDictionary in CurrentRatesData
        ///     Data will be prepared for future/possible features
        ///     Call this function only if there is possibility of new rates, which is according to
        ///     cnb documentation every day after 2:30pm, otherwise use same data.
        /// </summary>
        private void LoadCurrentRates()
        {
            var newRates = new CurrentRatesData();
            using (var client = new WebClient())
            {
                foreach (var source in DataSource)
                {
                    var response = client.DownloadString(source).Split('\n');
                    var match = Regex.Match(response[0], @"^([\w\s]+)#.+$");
                    if (!match.Success) continue;
                    newRates.UpdateDate(match.Groups[1].Value);
                    for (var i = 2; i < response.Length; i++)
                    {
                        var splitRow = response[i].Split('|');
                        if (splitRow.Length != 5) continue;
                        var amount = int.Parse(splitRow[2]);
                        var code = splitRow[3];
                        var rate = decimal.Parse(splitRow[4]);
                        if (newRates.Currencies.ContainsKey(code)) continue;
                        newRates.Currencies.Add(code, rate / amount);
                    }
                }
            }
            _currentRates = newRates;
        }

        /// <summary>
        ///     Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        ///     by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        ///     do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        ///     some of the currencies, ignore them.
        ///     Check date and time of last downloaded data and call LoadCurrentRates if needed.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var dateNow = DateTime.Now;
            var updateTime = TimeSpan.Parse("14:30");
            var daysFromLastUpdate = (dateNow.Date - _currentRates.Date).TotalDays;
            if (daysFromLastUpdate > 1 || daysFromLastUpdate > 0 && dateNow.TimeOfDay > updateTime)
                LoadCurrentRates();

            var targetCurrency = new Currency("CZK");
            return from currency in currencies
                where _currentRates.Currencies.ContainsKey(currency.Code)
                select new ExchangeRate(currency, targetCurrency, _currentRates.Currencies[currency.Code]);
        }
    }
}