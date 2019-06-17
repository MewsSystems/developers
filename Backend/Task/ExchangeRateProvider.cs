using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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
        /// 

        private const string CZECH_CURRENCY = "CZK";

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var listCurrencies = currencies.ToList();

            if (listCurrencies.Exists(c => c.Code.Equals(CZECH_CURRENCY)) == false)
                throw new ArgumentException($"Specified currencies does not contain Czech currency({CZECH_CURRENCY})");

            var cnbData = CnbClient.GetCnbRates();
            return GetRatesFromString(cnbData, listCurrencies);
        }


        internal static List<ExchangeRate> GetRatesFromString(string cnbData, List<Currency> currencies)
        {
            if (cnbData == null) throw new ArgumentNullException(nameof(cnbData));
            if (string.IsNullOrWhiteSpace(cnbData))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(cnbData));

            // Split data files to lines
            var lines = cnbData.Split('\n');
            if (lines.Length < 3)
                throw new FormatException($"Invalid data file format - too few lines. Expected at least 3, got {lines.Length}.");

            // Process lines
            var rateList = new List<ExchangeRate>();
            for (int i = 2; i < lines.Length; i++)  // skip first two lines. Real data starts on third
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(lines[i])) continue;
                    var data = lines[i].Split('|');
                    if (data.Length != 5) throw new FormatException($"Expected 5 segments, got {data.Length}.");

                    var rate = decimal.Parse(data[4], CultureInfo.GetCultureInfo("cs-CZ"));
                    var amount = int.Parse(data[2]);

                    if (currencies.Exists(c => c.Code.Equals(data[3])))
                        rateList.Add(new ExchangeRate(new Currency(data[3]), new Currency(CZECH_CURRENCY), rate / amount));
                }
                catch (Exception e)
                {
                    throw new FormatException($"Invalid file format - Error on line {i + 1}: \"{lines[i]}\" cannot be parsed as exchange rate.", e);
                }

            }
            return rateList;
        }
    }
}
