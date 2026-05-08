using ExchangeRateUpdater.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Application.Interfaces
{
    /// <summary>
    /// Defines a contract for fetching exchange rates from an external data source.
    /// </summary>
    internal interface IExchangeRateService
    {
        /// <summary>
        /// Asynchronously gets exchange rates for the specified currencies from the provider.
        /// </summary>
        /// <param name="currencies"> A collection of <see cref="Currency"/> objects to get exchange rates for.</param>
        /// <returns>
        /// A task that contains a collection of <see cref="ExchangeRate"/> objects.
        /// </returns>
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
    }
}
