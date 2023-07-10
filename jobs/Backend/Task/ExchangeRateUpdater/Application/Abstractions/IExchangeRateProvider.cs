using ExchangeRateUpdater.CnbProvider.Enums;
using ExchangeRateUpdater.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Application.Abstractions
{
    public interface IExchangeRateProvider
    {
        /// <summary>
        /// Gets a collection of exchange rates provided by different sources
        /// </summary>
        /// <param name="currencies"></param>
        /// <param name="date"></param>
        /// <returns>A colletion of exchange rates</returns>
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, DateTime date);
    }
}
