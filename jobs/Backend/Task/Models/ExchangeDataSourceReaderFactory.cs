using ExchangeRateProvider.Services;
using System;

namespace ExchangeRateProvider.Models
{
    /// <summary>
    /// The factory for creating the exchange rate data source reader.
    /// </summary>
    public class ExchangeDataSourceReaderFactory
    {
        public static BaseExchangeDataSourceReader CreateDataSourceReader(ExchangeRateDataSourceType dataSourceType)
        {
            switch (dataSourceType)
            {
                case ExchangeRateDataSourceType.Cnb:
                    return new CnbExchangeDataSourceReader();
                default:
                    throw new NotSupportedException($"Data source type '{dataSourceType}' is not supported.");
            }
        }
    }
}
