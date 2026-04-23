using ExchangeRateUpdater.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Interfaces
{
    /// <summary>
    /// Provides functionality to retrieve currency exchange rates from a specified source.
    /// </summary>
    public interface IExchangeRateProvider
    {
        /// <summary>
        /// Retrieves exchange rates for the specified currencies from the provider's source.
        /// </summary>
        /// <param name="currencies">The collection of currencies to get exchange rates for.</param>
        /// <returns></returns>
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
    }
}
