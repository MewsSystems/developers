using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Services
{
    public class CzechNationalBankExchangeRateParser : ICzechNationalBankExchangeRateParser
    {
        private const int INDEX_AMOUNT = 2;
        private const int INDEX_CODE = 3;
        private const int INDEX_VALUE = 4;

        public IEnumerable<ExchangeRate> ConvertToExchangeRates(IAsyncEnumerable<string> linesWithExchangeRates)
        {
            return linesWithExchangeRates.Select(x => Parse(x)).Where(x => x != null).ToEnumerable();
        }

        private ExchangeRate Parse(string line)
        {
            return TryRetrieveParts(line, out int amount, out decimal value, out string currencyCode) ?
                   new ExchangeRate(new Currency(currencyCode), new Currency("CZK"), value / amount) : null;
        }

        private bool TryRetrieveParts(string line, out int amount, out decimal value, out string currencyCode)
        {
            amount = 0;
            value = 0;
            currencyCode = "";

            var parts = line.Split('|');
            return parts.Length == 5 &&
                int.TryParse(parts[INDEX_AMOUNT], out amount) &&
                decimal.TryParse(parts[INDEX_VALUE], out value) &&
                amount > 0 &&
                value > 0 &&
                (currencyCode = parts[INDEX_CODE]).Length == 3;
        }
    }
}
