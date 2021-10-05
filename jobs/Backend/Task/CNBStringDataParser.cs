using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    internal class CNBStringDataParser
    {
        private char[] _linesSeparator = new[] { '\n' };
        private char[] _valuesSeparator = new[] { '|' };

        public IEnumerable<ExchangeRate> Parse(string stringData)
        {
            var lines = stringData.Split(_linesSeparator, StringSplitOptions.RemoveEmptyEntries).Skip(2);
            foreach (var line in lines)
            {
                string[] lineValues = line.Split(_valuesSeparator, StringSplitOptions.RemoveEmptyEntries);
                var exchangeRate = new ExchangeRate(
                    new Currency(lineValues[3]),
                    new Currency("CZK"),
                    decimal.Parse(lineValues[4], CultureInfo.GetCultureInfo("cs-CZ").NumberFormat),
                    int.Parse(lineValues[2], CultureInfo.GetCultureInfo("cs-CZ").NumberFormat));

                yield return exchangeRate;
            }
        }
    }
}
