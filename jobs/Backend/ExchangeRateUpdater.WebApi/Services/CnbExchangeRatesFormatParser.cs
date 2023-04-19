using ExchangeRateUpdater.WebApi.Models;
using ExchangeRateUpdater.WebApi.Services.Interfaces;
using System.Globalization;

namespace ExchangeRateUpdater.WebApi.Services
{
    public class CnbExchangeRatesFormatParser : IExchangeRatesParser
    {
        public IEnumerable<ExchangeRate> ParseExchangeRates(string formattedExchangeRates)
        {
            IEnumerable<ExchangeRate> exchangeRates = Enumerable.Empty<ExchangeRate>();

            var currentExchangeRatesSeparated = formattedExchangeRates.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Skip(2);

            var currentExchangeRatesFields = currentExchangeRatesSeparated.Select(fullExchangeRate => fullExchangeRate.Split('|')).ToList();

            foreach (var currentExchangeRateField in currentExchangeRatesFields)
            {
                try
                {
                    var exchangeRate = new ExchangeRate(new Currency(Currency.CzechCurrencyCode),
                                                          new Currency(currentExchangeRateField[3]),
                                                          decimal.Parse(currentExchangeRateField[4], CultureInfo.InvariantCulture));
                    exchangeRates = exchangeRates.Append(exchangeRate);
                }
                catch (FormatException)
                {
                    throw new FormatException($"Exchange rate source file has the wrong format: {formattedExchangeRates}");
                }
            }

            return exchangeRates;
        }
    }
}
