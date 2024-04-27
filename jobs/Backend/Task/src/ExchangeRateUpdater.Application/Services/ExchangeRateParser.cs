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
        public List<ExchangeRate> Parse(string sourceCurrencyCode, string data)
        {
            var exchangeRates = new List<ExchangeRate>();
            string[] lines = data.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 2; i < lines.Length; i++) // We are skipping the first two lines
            {
                string[] properties = lines[i].Split('|');

                if (properties.Length == 5)
                {
                    string CountryName = properties[0].Trim();
                    string currencyName = properties[1].Trim();
                    int amount = int.Parse(properties[2]);
                    string currencyCode = properties[3].Trim();
                    decimal exchangeRate = decimal.Parse(properties[4].Replace(",", "."), CultureInfo.InvariantCulture);

                    var exchange = new ExchangeRate
                    {
                        CountryName = CountryName,
                        TargetCurrencyCode = currencyCode,
                        SourceCurrencyCode = sourceCurrencyCode,
                        TargetCurrencyName = currencyName,
                        Amount = amount,
                        Value = exchangeRate
                    };

                    exchangeRates.Add(exchange);
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
