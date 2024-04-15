namespace ExchangeRateUpdater.Infrastructure.Api.CzechNationalBankApi.Dto
{
    /// <summary>
    /// Represents exchange rate data retrieved from a REST API endpoint.
    /// </summary>
    internal class ExDateDailyRest
    {
        /// <summary>
        /// Amount.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Country name.
        /// </summary>
        public string Country { get; set; } = "";

        /// <summary>
        /// Currency name.
        /// </summary>
        public string Currency { get; set; } = "";

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string CurrencyCode { get; set; } = "";

        /// <summary>
        /// Order.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Exchange rate.
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// Date the exchange rate is valid for.
        /// </summary>
        public DateOnly ValidFor { get; set; }
    }
}
