using ExchangeRateFinder.Infrastructure.Models;
using Microsoft.Extensions.Logging;
using System.Globalization;
namespace ExchangeRateFinder.Infrastructure.Services
{
    public interface IExchangeRateParser
    {
        List<ExchangeRate> Parse(string sourceCurrencyCode, string data);
    }
    public class ExchangeRateParser : IExchangeRateParser
    {
        private readonly ILogger<ExchangeRateParser> _logger;

        public ExchangeRateParser(ILogger<ExchangeRateParser> logger)
        {
            _logger = logger;
        }
        public List<ExchangeRate> Parse(string targetCurrencyCode, string data)
        {
            var exchangeRates = new List<ExchangeRate>();
            string[] lines = data.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 2; i < lines.Length; i++) // We are skipping the first two lines
            {
                string[] properties = lines[i].Split('|');

                if (properties.Length == 5)
                {
                    string countryName = properties[0].Trim();
                    string currencyName = properties[1].Trim();
                    int amount = int.Parse(properties[2]);
                    string sourceCurrencyCode = properties[3].Trim();
                    decimal exchangeRate = decimal.Parse(properties[4].Replace(",", "."), CultureInfo.InvariantCulture);

                    if (!string.IsNullOrEmpty(countryName) &&
                        !string.IsNullOrEmpty(currencyName) &&
                        !string.IsNullOrEmpty(sourceCurrencyCode) &&
                        amount > 0 &&
                        exchangeRate > 0) 
                    { 
                        var exchange = new ExchangeRate
                        {
                            CountryName = countryName,
                            TargetCurrencyCode = targetCurrencyCode,
                            SourceCurrencyCode = sourceCurrencyCode,
                            SourceCurrencyName = currencyName,
                            Amount = amount,
                            Value = exchangeRate
                        };

                        exchangeRates.Add(exchange);
                    }
                }
                else
                {
                    _logger.LogError($"{typeof(ExchangeRateParser)} unable to process fields in line {i}: {lines[i]}");
                }
            }

            return exchangeRates;
        }
    }
}
