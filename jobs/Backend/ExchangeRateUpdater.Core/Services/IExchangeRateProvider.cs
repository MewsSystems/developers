using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateUpdater.Core.Entities;

namespace ExchangeRateUpdater.Core.Services
{
    /// <summary>
    /// Interface for retrieving exchange rates from a data source.
    /// </summary>
    public interface IExchangeRateProvider
    {
        /// <summary>
        /// Gets exchange rates for the specified currencies and date.
        /// </summary>
        IAsyncEnumerable<ExchangeRate> GetExchangeRatesAsync(IEnumerable<Currency> currencies, DateTime date);
    }
}
