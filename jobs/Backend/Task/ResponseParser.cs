using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System;

namespace ExchangeRateUpdater
{
    internal class ResponseParser
    {
        private const int headerLinesCount = 2;
        private const int amountPosition = 2;
        private const int codePosition = 3;
        private const int ratePosition = 4;
        private char[] linesDelimiter = new[] { '\n' };
        private char[] valuesDelimiter = new[] { '|' };
        private Currency targetCurrency = new Currency("CZK");

        public IEnumerable<ExchangeRate> ParseResponseFromSource(string responseBody)
        {
            var exchangeRatesList = new List<ExchangeRate>();

            var responseLines = responseBody.Split(linesDelimiter, StringSplitOptions.RemoveEmptyEntries).Skip(headerLinesCount);

            foreach (var responseLine in responseLines)
            {
                var responseValues = responseLine.Split(valuesDelimiter, StringSplitOptions.RemoveEmptyEntries);

                var sourceCurrency = new Currency(responseValues[codePosition]);
                var rate = decimal.Parse(responseValues[ratePosition], CultureInfo.InvariantCulture.NumberFormat);
                var amount = int.Parse(responseValues[amountPosition], CultureInfo.InvariantCulture.NumberFormat);

                exchangeRatesList.Add(
                    new ExchangeRate(
                        sourceCurrency,
                        targetCurrency,
                        rate,
                        amount));                        
            }

            return exchangeRatesList;
        }
    }
}
