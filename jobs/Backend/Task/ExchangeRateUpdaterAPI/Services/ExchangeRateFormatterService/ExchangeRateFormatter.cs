using System.Globalization;
using ExchangeRateUpdater;

namespace ExchangeRateUpdaterAPI.Services.ExchangeRateFormatterService
{
    public class ExchangeRateFormatter : IExchangeRateFormatter
    {
        public IEnumerable<ExchangeRate> FormatExchangeRates(string exchangeRateData)
        {
            try
            {
                return exchangeRateData
                    .Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .Skip(2)
                    .Select(line => line.Split('|'))
                    .Select(columns => new ExchangeRate(
                        new Currency(Currency.CzechRepublicCurrencyCode),
                        new Currency(columns[3]),
                        decimal.Parse(columns[4], CultureInfo.InvariantCulture)
                    ));
            }
            catch (Exception ex)
            {
                throw new FormatException($"Something went wrong. Please check the file format is correct.: {exchangeRateData}", ex);
            }
        }
    }
}

