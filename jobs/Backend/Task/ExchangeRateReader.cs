using System;
using System.Collections.Generic;
using System.IO;

namespace ExchangeRateUpdater
{
    public class ExchangeRateReader
    {
        public IEnumerable<ExchangeRate> ReadExchangeRates(string exchangeRateResponse)
        {
            var reader = new StringReader(exchangeRateResponse);
            SkipHeader(reader);

            var exchangeRates = new List<ExchangeRate>();
            AddRemaingLinesToExchangeRates(reader, exchangeRates);

            return exchangeRates;
        }

        private void SkipHeader(StringReader reader)
        {
            reader.ReadLine();
            reader.ReadLine();
        }

        private void AddRemaingLinesToExchangeRates(StringReader reader, List<ExchangeRate> exchangeRates)
        {
            var currentLine = reader.ReadLine();

            if (currentLine is null)
            {
                return;
            }

            var exchangeRateFields = currentLine.Split('|');

            var amount = Convert.ToInt32(exchangeRateFields[2]);
            var currency = exchangeRateFields[3];
            var value = Convert.ToDecimal(exchangeRateFields[4]) / amount;

            var exchangeRate = new ExchangeRate(new Currency(currency), new Currency("CZK"), value);
            exchangeRates.Add(exchangeRate);

            AddRemaingLinesToExchangeRates(reader, exchangeRates);
        }
    }
}
