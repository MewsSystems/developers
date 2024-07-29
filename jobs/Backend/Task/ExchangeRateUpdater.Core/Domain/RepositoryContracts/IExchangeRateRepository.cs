using ExchangeRateUpdater.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Domain.RepositoryContracts
{
    public interface IExchangeRateRepository
    {
        /// <summary>
        /// Gets all Exchange Rates from the data repository
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <param name="requestUrl"></param>
        /// <returns>Returns list of current exchange rates</returns>
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(string currencyCode, string requestUrl);
    }
}
