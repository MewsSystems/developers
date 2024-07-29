using ExchangeRateUpdater.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Domain.RepositoryContracts
{
    public interface ICurrencySourceRepository
    {
        /// <summary>
        /// Gets all Currency Sources from the data repository
        /// </summary>
        /// <returns>Returns a list of the currency sources currently stored in the database</returns>
        Task<List<CurrencySource>> GetCurrencySourcesAsync();

        /// <summary>
        /// Gets Currency Source for a specified Currency Code
        /// </summary>
        /// <param name="currencyCode">3 character string for the specific currency code to query the CurrencySource against</param>
        /// <returns>Returns either the CurrencySource that matches or null if none is found.</returns>
        Task<CurrencySource?> GetCurrencySourceByCurrencyCodeAsync(string currencyCode);
    }
}
