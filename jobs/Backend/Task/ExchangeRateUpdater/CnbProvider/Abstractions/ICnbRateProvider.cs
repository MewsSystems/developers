using ExchangeRateUpdater.Domain;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.CnbProvider.Abstractions
{
    public interface ICnbRateProvider
    {
        /// <summary>
        /// Gets a collection of exchange rates provided by Cbn
        /// </summary>
        /// <param name="date"></param>
        /// <returns>A colletion of exchange rates</returns>
        Task<IEnumerable<ExchangeRate>> GetRatesByDateAsync(DateTime date);
    }
}