using System.Collections.Generic;
using ExchangeRateUpdater.DataFetchers;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Parsers;
using ExchangeRateUpdater.Utils;

namespace ExchangeRateUpdater.Services
{
    /// <summary>
    /// Provides functionality to fetch, parse, and filter exchange rates based on specified currencies.
    /// </summary>
    public class ExchangeRateService
    {
        private readonly IRemoteDataFetcher _dataFetcher;

        private readonly IParser _parser;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRateService"/> class.
        /// </summary>
        /// <param name="dataFetcher">The <see cref="IRemoteDataFetcher"/> implementation used to fetch data.</param>
        /// <param name="parser">The <see cref="IParser"/> implementation used to parse data.</param>
        public ExchangeRateService(IRemoteDataFetcher dataFetcher, IParser parser)
        {
            _dataFetcher = dataFetcher;
            _parser = parser;
        }

        /// <summary>
        /// Fetches raw data, parses exchange rates, and filters them by the given currencies.
        /// </summary>
        /// <param name="currencies">IEnumerable of <see cref="Currency"/> to find exchange rates for.</param>
        /// <returns>An <see cref="IEnumerable{ExchangeRate}"/> matching the specified currencies.</returns>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            string fileData = _dataFetcher.FetchData();
            IEnumerable<ExchangeRate> exchangeRates = _parser.ParseData(fileData);

            return ExchangeRateFilter.FilterByCurrencies(exchangeRates, currencies);
        }
    }
}
