namespace ExchangeRateProvider.Models
{
    public abstract class BaseExchangeDataSource
    {
        /// <summary>
        /// The source currency code.
        /// </summary>
        public abstract SourceCurrencyCode SourceCurrencyCode { get; }

        /// <summary>
        /// The type of the exchange rate data source.
        /// </summary>
        public abstract ExchangeRateDataSourceType DataSourceType { get; }

        /// <summary>
        /// The URL to connect to the exchange rate data source.
        /// </summary>
        public abstract string ConnectionUrl { get; }

        /// <summary>
        /// Returns the URL to fetch the exchange rate dataset.
        /// </summary>
        /// <returns></returns>
        public abstract string GetExchangeRateDatasetUrl();
    }

    /// <summary>
    /// The source currency code.
    /// </summary>
    public enum SourceCurrencyCode
    {
        CZK = 1
    }
}
