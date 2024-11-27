using ExchangeRateUpdater.Interfaces;
using System;

namespace ExchangeRateProvider.Models
{
    public class ExchangeDataSourceFactory : IExchangeDataSourceFactory
    {
        /// <summary>
        /// Creates a new instance of the exchange rate data source.
        /// </summary>
        /// <param name="dataSourceType"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public BaseExchangeDataSource CreateDataSource(ExchangeRateDataSourceType dataSourceType)
        {
            switch (dataSourceType)
            {
                case ExchangeRateDataSourceType.Cnb:
                    return new CnbExchangeRateDataSource();
                default:
                    throw new NotSupportedException($"Data source type '{dataSourceType}' is not supported.");
            }
        }
    }

    /// <summary>
    /// The type of the exchange rate data source.
    /// </summary>
    public enum ExchangeRateDataSourceType
    {
        Cnb = 1
    }
}
