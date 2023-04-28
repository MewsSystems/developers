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
                if (exchangeRateData == string.Empty)
                {
                    throw new FormatException($"File is empty");
                }
                return exchangeRateData
                    .Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .Skip(2)
                    .Select(line => line.Split('|'))
                    .Select(columns =>
                    {
                        if (columns.Length < 5)
                        {
                            throw new FormatException($"Missing columns in line: {string.Join("|", columns)}");
                        }
                        if (!decimal.TryParse(columns[4], NumberStyles.Float, CultureInfo.InvariantCulture, out decimal exchangeRateValue))
                        {
                            throw new FormatException($"Invalid decimal value: {columns[4]}");
                        }

                        return new ExchangeRate(
                            new Currency(Currency.CzechRepublicCurrencyCode),
                            new Currency(columns[3]),
                            exchangeRateValue
                        );
                    });
            }
            catch (Exception ex)
            {
                throw new FormatException($"Something went wrong. Please check the file format is correct.: {exchangeRateData}", ex);
            }
        }
    }
}

