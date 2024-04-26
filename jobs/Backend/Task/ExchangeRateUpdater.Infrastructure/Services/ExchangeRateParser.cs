using ExchangeRateFinder.Infrastructure.Models;
using System.Globalization;
namespace ExchangeRateFinder.Infrastructure.Services
{
    public interface IExchangeRateParser
    {
        List<ExchangeRate> Parse(string sourceCurrency, string data);
    }
    public class ExchangeRateParser : IExchangeRateParser
    {
        public List<ExchangeRate> Parse(string sourceCurrency, string data)
        {
            var exchangeRates = new List<ExchangeRate>();
            string[] lines = data.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 2; i < lines.Length; i++)
            {
                string[] properties = lines[i].Split('|');

                if (properties.Length == 5)
                {
                    string country = properties[0].Trim();
                    string currency = properties[1].Trim();
                    int amount = int.Parse(properties[2]);
                    string currencyCode = properties[3].Trim();
                    decimal exchangeRate = decimal.Parse(properties[4].Replace(",", "."), CultureInfo.InvariantCulture);

                    var exchange = new ExchangeRate
                    {
                        Country = country,
                        TargetCurrency = currency,
                        SourceCurrency = sourceCurrency,
                        Amount = amount,
                        Code = currencyCode,
                        Rate = exchangeRate
                    };

                    exchangeRates.Add(exchange);
                }
                else
                {
                    throw new FormatException($"Invalid number of fields in line {i}: {lines[i]}");
                }
            }

            return exchangeRates;
        }
    }
}
