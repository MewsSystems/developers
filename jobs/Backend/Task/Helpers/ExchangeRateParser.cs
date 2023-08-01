using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services.Responses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Helpers
{
    public class ExchangeRateParser : IExchangeRateParser
    {
        public ExchangeRatesResponse ParseRatesFromApi(string response)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<ExchangeRatesResponse>(response, options);
        }

        public List<ExchangeRate> ParseRatesFromText(IEnumerable<Currency> currencies, string[] lines)
        {
            var rates = new List<ExchangeRate>();
            foreach (var line in lines)
            {
                var columns = line.Split('|');
                if (columns.Length < 5)
                {
                    continue;
                }

                var currencyCode = columns[3];
                if (!decimal.TryParse(columns[2], NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
                {
                    continue;
                }

                if (currencies.Any(c => c.Code == currencyCode))
                {
                    if (!decimal.TryParse(columns[4], NumberStyles.Any, CultureInfo.InvariantCulture, out var rate))
                    {
                        continue;
                    }
                    rate /= amount; 
                    rates.Add(new ExchangeRate(new Currency("CZK"), new Currency(currencyCode), rate));
                }
            }

            return rates;

        }

        public List<ExchangeRate> GetRatesFromData(IEnumerable<Currency> currencies, ExchangeRatesResponse data)
        {
            var rates = new List<ExchangeRate>();
            foreach (var rate in data.Rates)
            {
                if (currencies.Any(c => c.Code == rate.CurrencyCode))
                {
                    var exchangeRate = new ExchangeRate(new Currency("CZK"), new Currency(rate.CurrencyCode), rate.ExchangeRateValue / rate.Amount);
                    rates.Add(exchangeRate);
                }
            }

            return rates;
        }
    }

}
