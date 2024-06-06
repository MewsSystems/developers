namespace ExchangeRateUpdater.ExchangeRate.Provider.CzechNationalBank
{
    /// <summary>
    /// Configuration for Czech National Bank API.
    /// </summary>
    internal class CzechNationalBankConfig
    {
        /// <summary>
        /// Gets or sets the base URL of the Czech National Bank API.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the endpoint for fetching daily exchange rates.
        /// </summary>
        public string DailyExchangeRateEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the currency code of the source currency.
        /// </summary>
        public string SourceCurrencyCode { get; set; }
    }
}
