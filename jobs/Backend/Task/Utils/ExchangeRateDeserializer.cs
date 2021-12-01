using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ExchangeRateUpdater
{
    public static class ExchangeRateDeserializer
    {
        private const char LineSeparator = '\n';
        private const char LineValuesSeparator = '|';
        private const int CurrencyCodeArrayPosition = 3;
        private const int RateArrayPosition = 4;
        private const int NumberOfHeaderRows = 2;

        public static IEnumerable<ExchangeRate> DeserializeCNBExchangeRateString(string input)
        {
            var output = new List<ExchangeRate>();
            var exchangeRate = new ExchangeRate();

            var inputLines = input.Split(LineSeparator, StringSplitOptions.RemoveEmptyEntries).Skip(NumberOfHeaderRows);

            foreach (var line in inputLines)
            {
                var lineValues = line.Split(LineValuesSeparator);

                exchangeRate = new ExchangeRate(
                    new Currency("CZK"), 
                    new Currency(lineValues[CurrencyCodeArrayPosition]),
                    decimal.Parse(lineValues[RateArrayPosition], CultureInfo.GetCultureInfo("cs-CZ")));

                output.Add(exchangeRate);
            }

            return output;
        }
    }
}
