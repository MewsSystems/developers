using ExchangeRateProvider.Models;
using ExchangeRateUpdater;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateProvider.Services
{
    public abstract class BaseExchangeDataSourceReader
    {
        /// <summary>
        /// Fetches and parses the exchange rates from the data source.
        /// </summary>
        protected BaseExchangeDataSource _baseExchangeDataSource;

        /// <summary>
        /// Fetches and parses the exchange rates from the data source.
        /// </summary>
        /// <param name="baseExchangeDataSource"></param>
        /// <returns></returns>
        public abstract Task<List<ExchangeRate>> FetchAndParseExchangeRatesAsync(BaseExchangeDataSource baseExchangeDataSource);

    }
}
