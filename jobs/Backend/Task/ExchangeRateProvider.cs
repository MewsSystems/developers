using ExchangeRateProvider.Models;
using ExchangeRateUpdater.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {/// <summary>
     /// The data source to fetch exchange rates from.
     /// </summary>
        private BaseExchangeDataSource _exchangeDataSource;


        public ExchangeRateProvider(BaseExchangeDataSource baseExchangeDataSource)
        {
            _exchangeDataSource = baseExchangeDataSource;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            try
            {
                var textFormatUrl = _exchangeDataSource.GetExchangeRateDatasetUrl();

                var readerService = ExchangeDataSourceReaderFactory.CreateDataSourceReader(_exchangeDataSource.DataSourceType);
                // Fetch and parse the data
                var exchangeRates = readerService.FetchAndParseExchangeRatesAsync(_exchangeDataSource)
                    .GetAwaiter()
                    .GetResult();

                // Filter the exchange rates based on the specified currencies
                return exchangeRates.Where(rate =>
                    currencies.Any(c => c.Code == rate.TargetCurrency.Code));
            }
            catch (Exception ex)
            {
                // return a specific error
                throw new Exception("An error occurred while fetching exchange rates.", ex);
            }
        }
    }
}
