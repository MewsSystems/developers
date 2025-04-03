using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateParser
    {
        public static List<ExchangeRate> Parse(string data)
        {
            var lines = data.Trim().Split('\n');
            var currencyLines = lines.Skip(2); // skip headers

            var sourceCurrency = new Currency("CZK");

            var exchangeRates = new List<ExchangeRate>();

            foreach (var line in currencyLines)
            {
                var parts = line.Split('|');

                var targetCurrency = new Currency(parts[3].Trim());

                // rate
                if (!TryParseDecimalInvariant(parts[4].Trim(), out decimal rate))
                {
                    continue;
                }

                // amount
                if (!TryParseDecimalInvariant(parts[2].Trim(), out decimal amount))
                {
                    continue;
                }

                exchangeRates.Add(new ExchangeRate(sourceCurrency, targetCurrency, rate / amount));
            }

            return exchangeRates;
        }

        private static bool TryParseDecimalInvariant(string decimalString, out decimal value)
        {
            return decimal.TryParse(decimalString.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out value);
        }
    }
}
