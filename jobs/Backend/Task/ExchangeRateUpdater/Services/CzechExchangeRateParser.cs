using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Services
{
    public class CzechExchangeRateParser : IExchangeRateParser
    {
        public IEnumerable<ExchangeRate> ParseExchangeRates(string cnbText, Currency targetCurrency, IEnumerable<Currency> currencies)
        {
            ArgumentNullException.ThrowIfNull(cnbText);
            var lines = cnbText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).Skip(Constants.CNBParserRowToSkip); // Skip header lines
            var hashSetCurrencies = new HashSet<string>(currencies.Select(x => x.Code));

            foreach (var line in lines)
            {
                var (currencyCode, value) = ParseExchangeRateData(line);

                if (hashSetCurrencies.Contains(currencyCode))
                {
                    var baseCurrency = new Currency(currencyCode);
                    yield return new ExchangeRate(baseCurrency, targetCurrency, value);
                }
            }
        }

        private (string, decimal) ParseExchangeRateData(string line)
        {
            try
            {
                var columns = line.Split(Constants.CNBParserColumnSeparator);
                var currencyCode = columns[3].Trim();
                var exchangeRate = decimal.Parse(columns[4].Trim(), CultureInfo.InvariantCulture);
                var quantity = int.Parse(columns[2].Trim(), CultureInfo.InvariantCulture);
                return (currencyCode, exchangeRate / quantity);
            }
            catch (FormatException ex)
            {
                throw new InvalidOperationException("Failed to parse exchange rate data due to an invalid format.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An unexpected error occurred while parsing exchange rate data.", ex);
            }
        }
    }
}
