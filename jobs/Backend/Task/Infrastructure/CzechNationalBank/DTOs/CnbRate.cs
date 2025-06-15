namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank.DTOs
{
    /// <summary>
    /// Represents a single exchange rate entry from the Czech National Bank API.
    /// </summary>
    public class CnbRate
    {
        /// <summary>
        /// The date for which the exchange rate is valid.
        /// </summary>
        public string ValidFor { get; set; }

        /// <summary>
        /// The order number
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// The country associated with the currency.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// The name of the currency.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// The amount of the currency in the comparison.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// The ISO 4217 code of the currency.
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// The exchange rate value.
        /// </summary>
        public decimal Rate { get; set; }
    }
}
