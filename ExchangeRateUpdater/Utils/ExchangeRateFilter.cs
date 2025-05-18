using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Utils
{
    /// <summary>
    /// Provides methods to filter exchange rates based on given criteria.
    /// </summary>
    public class ExchangeRateFilter
    {
        /// <summary>
        /// Filters a collection of exchange rates to include only those matching the specified currencies.
        /// </summary>
        /// <param name="exchangeRates">The collection of exchange rates to filter.</param>
        /// <param name="currencies">The currencies used to filter exchange rates. Cannot be null nor empty.</param>
        /// <returns>An <see cref="IEnumerable{ExchangeRate}"/> matching the specified currencies.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="currencies"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="currencies"/> is empty.</exception>
        public static IEnumerable<ExchangeRate> FilterByCurrencies(IEnumerable<ExchangeRate> exchangeRates, IEnumerable<Currency> currencies)
        {
            if (currencies is null)
            {
                throw new ArgumentNullException(nameof(currencies), "Currencies cannot be null.");
            }

            if (!currencies.Any())
            {
                throw new ArgumentException("Currencies cannot be empty.", nameof(currencies));
            }

            IEnumerable<ExchangeRate> exchangeRatesResult = new List<ExchangeRate>();

            return exchangeRates
                .Where(er => currencies.Any(c => c.Code == er.SourceCurrency.Code))
                .ToList();
        }
    }
}