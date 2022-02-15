using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Helpers
{
    public static class CurrencyHelper
    {
        public static IEnumerable<Currency> GetCurrencies(IConfiguration configuration)
        {
            var currencies = configuration.GetSection("TargetCurrencies").Get<string[]>();
            return currencies.ToList().Select(currency => new Currency(currency));
        }
    }
}
