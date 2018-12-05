using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ExchangeRateUpdater
{
    public interface IExchangeRatesParser
    {
        IDictionary<string, decimal> ParseRatesData(string rawData);
    }

    internal class ExchangeRatesParser : IExchangeRatesParser
    {
        public IDictionary<string, decimal> ParseRatesData(string rawData)
        {
            string[] dataSplitted = rawData.Split(new[] { '\n' });
            char[] rowSplitter = new[] { '|' };
            CultureInfo czechCultureInfo = new CultureInfo("cs-CZ");
            IDictionary<string, decimal> result = new Dictionary<string, decimal>();

            foreach (string row in dataSplitted.Skip(2))
            {
                string[] rowSplitted = row.Split(rowSplitter);
                if (rowSplitted.Length == 5)
                {
                    string isoCode = rowSplitted[3];
                    decimal rate = decimal.Parse(rowSplitted[4], NumberStyles.Currency, czechCultureInfo);
                    decimal amount = decimal.Parse(rowSplitted[2], NumberStyles.Currency, czechCultureInfo);

                    result.Add(
                        isoCode,
                        rate / amount);
                }
            }

            return result;
        }
    }
}
