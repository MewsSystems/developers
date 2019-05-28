using System;
using System.Diagnostics;

namespace ExchangeRateUpdater
{
    public class CNBExchangeRateParsingStrategy : IExchangeRateParsingStrategy
    {
        private readonly Currency targetCurrency = new Currency("CZK");

        public ExchangeRate Parse(string line)
        {
            var lineParts = line.Split('|');
            var sourceCurrency = new Currency(lineParts[3]);
            var value = decimal.Parse(lineParts[4]);
            var amount = int.Parse(lineParts[2]);

            return new CNBExchangeRate(sourceCurrency, targetCurrency, value, amount);
        }

        public bool TryParse(string line, out ExchangeRate exchangeRate)
        {
            exchangeRate = null;

            try
            {
                exchangeRate = this.Parse(line);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }

            return exchangeRate != null;
        }
    }
}
