using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Constants;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using Newtonsoft.Json;

namespace ExchangeRateUpdater.Services
{
    public class CnbExchangeRateParser : IExchangeRateParser
    {
        public IEnumerable<ExchangeRate> Parse(string data, IEnumerable<Currency> currencies)
        {
            if (string.IsNullOrWhiteSpace(data) || currencies == null || !currencies.Any())
                return Enumerable.Empty<ExchangeRate>();

            var apiResponse = JsonConvert.DeserializeObject<ExchangeRateApiResponse>(data);

            return apiResponse.Rates
               .Where(rate => currencies.Any(c => c.Code == rate.CurrencyCode))
                .Select(rate => new ExchangeRate(new Currency(GeneralConstants.BaseCurrencyCode), new Currency(rate.CurrencyCode), rate.RateValue))
               .ToList();
        }
    }
}
