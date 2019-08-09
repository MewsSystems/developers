using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private static readonly CultureInfo BaseCultureInfo = CultureInfo.CreateSpecificCulture("cz-CZ");
        private static readonly TimeZoneInfo BaseTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        private static readonly Currency BaseCurrency = new Currency("CZK");

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var today = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, BaseTimeZone).Date;
            var currencyCodes = new HashSet<string>(currencies.Select(x => x.Code), StringComparer.InvariantCultureIgnoreCase);

            return
                ReadRates(BuildExchangeRatesFixingUrl(today), currencyCodes).Concat(
                ReadRates(BuildExchangeRatesOfOtherCurrenciesUrl(today), currencyCodes)).Concat(
                    currencyCodes.Contains(BaseCurrency.Code)
                        ? new[] { new ExchangeRate(BaseCurrency, BaseCurrency, 1) }
                        : new ExchangeRate[0]);
        }

        private IEnumerable<ExchangeRate> ReadRates(string url, HashSet<string> currencies)
        {
            var result = new List<ExchangeRate>();

            using (var response = WebRequest.Create(url).GetResponse())
            using (var stream = response.GetResponseStream())
            using (var streamReader = new StreamReader(stream))
            {
                string line;
                while (!string.IsNullOrEmpty(line = streamReader.ReadLine()))
                {
                    if (!TryExtractRateDetails(line, out var rateDetails))
                    {
                        continue;
                    }

                    if (currencies.Contains(rateDetails.curencyCode))
                    {
                        result.Add(new ExchangeRate(
                            new Currency(rateDetails.curencyCode),
                            BaseCurrency,
                            rateDetails.rate / rateDetails.amount));
                    }
                }
            }

            return result;
        }

        private bool TryExtractRateDetails(string line, out (string curencyCode, int amount, decimal rate) rateDetails)
        {
            var split = line.Split('|');
            if (split.Length != 5 ||
                !int.TryParse(split[2], out var amount) ||
                !decimal.TryParse(split[4], NumberStyles.Currency, BaseCultureInfo, out var rate))
            {
                rateDetails = default;
                return false;
            }

            rateDetails = (split[3], amount, rate);
            return true;
        }

        private string BuildExchangeRatesFixingUrl(DateTime today)
        {
            return $"https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date={today:dd+MMM+yyyy}";
        }

        private string BuildExchangeRatesOfOtherCurrenciesUrl(DateTime today)
        {
            return $"https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt?year={today.Year}&month={today.Month}";
        }
    }
}
