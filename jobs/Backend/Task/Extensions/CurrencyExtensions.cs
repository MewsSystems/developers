using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Extensions
{
    public static class CurrencyExtensions
    {
        /// <summary>
        /// Checks if the list of Currency contains a currency with the given code.
        /// </summary>
        public static bool Contains(this IEnumerable<Currency> currencies, string currencyCode)
        {
            if (currencyCode is null)
                throw new ArgumentNullException(nameof(currencyCode));

            return currencies.Any(c =>
                string.Equals(c.Code, currencyCode, StringComparison.OrdinalIgnoreCase));
        }
    }
}
