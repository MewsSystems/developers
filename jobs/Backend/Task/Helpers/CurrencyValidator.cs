using ExchangeRateUpdater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Helpers
{
    public class CurrencyValidator
    {
        public static void ValidateCurrencies(IEnumerable<Currency> currencies)
        {
            if (currencies == null || !currencies.Any())
            {
                throw new ValidationException("At least one currency must be specified.");
            }

            if (currencies.Any(c => c == null || string.IsNullOrWhiteSpace(c.Code)))
            {
                throw new ValidationException("Invalid currency specified.");
            }
        }
    }

}
