using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for fetching exchange rates from an external source.
    /// </summary>
    public interface IExchangeRateProvider
    {
        /// <summary>
        /// Asynchronously retrieves exchange rates for the specified currencies.
        /// </summary>
        /// <param name="currencies">A collection of <see cref="Currency"/> objects to fetch exchange rates for.</param>
        /// <returns>A task containing a collection of <see cref="ExchangeRate"/> objects.</returns>
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
    }
}
