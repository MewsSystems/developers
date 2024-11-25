using System;

namespace ExchangeRateProvider.Models
{
    public class CnbExchangeRateDataSource : BaseExchangeDataSource
    {
        /// <summary>
        /// The base URL of the exchange rate data source.
        /// </summary>
        private const string BaseUrl = "https://www.cnb.cz";

        /// <summary>
        /// The URL path to fetch the exchange rate dataset.
        /// </summary>
        private const string DailyTextUrl = "/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

        /// <summary>
        /// The date of the exchange rate. Default is today.
        /// </summary>
        private DateTime _exchangeRateDate;

        /// <summary>
        /// The date format.
        /// </summary>
        private string DATE_FORMAT = "dd.MM.yyyy";

        /// <summary>
        /// The type of the exchange rate data source.
        /// </summary>
        public override ExchangeRateDataSourceType DataSourceType => ExchangeRateDataSourceType.Cnb;

        /// <summary>
        /// The URL to connect to the exchange rate data source.
        /// </summary>
        public override string ConnectionUrl => $"{BaseUrl}{DailyTextUrl}";

        /// <summary>
        /// The date of the exchange rate.
        /// </summary>
        public DateTime ExchangeRateDate { get => _exchangeRateDate; set => _exchangeRateDate = value; }

        /// <summary>
        /// The source currency code.
        /// </summary>
        public override SourceCurrencyCode SourceCurrencyCode => SourceCurrencyCode.CZK;


        public CnbExchangeRateDataSource(DateTime? exchangeRateDate = null) : base()
        {
            ExchangeRateDate = exchangeRateDate ?? DateTime.UtcNow;
        }

        /// <summary>
        /// Returns the URL to fetch the exchange rate dataset.
        /// </summary>
        /// <returns></returns>
        public override string GetExchangeRateDatasetUrl()
        {
            var formattedDate = ExchangeRateDate.ToString(DATE_FORMAT);
            return $"{ConnectionUrl}?date={formattedDate}";
        }

        public override string ToString()
        {
            return "Czech National Bank (CNB)";
        }
    }
}
