using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateParser : IExchangeRateParser
    {
        public IEnumerable<ExchangeRate> ParseExchangeRates(string cnbText, Currency targetCurrency, IEnumerable<Currency> currencies)
        {
            var lines = cnbText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).Skip(2); // Skip header lines
            var exchangeRates = new List<ExchangeRate>();

            foreach (var line in lines)
            {
                var columns = line.Split('|');

                var currencyCode = columns[3].Trim();
                var exchangeRate = decimal.Parse(columns[4].Trim(), CultureInfo.InvariantCulture);
                var quantity = int.Parse(columns[2].Trim(), CultureInfo.InvariantCulture);

                if (currencies.Any(c => c.Code == currencyCode))
                {
                    var baseCurrency = new Currency(currencyCode);
                    var value = exchangeRate / quantity;

                    yield return new ExchangeRate(baseCurrency, targetCurrency, value);
                }
            }
        }
    }
}
