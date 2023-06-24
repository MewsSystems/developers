namespace ExchangeRateUpdater.Data
{
    /// <summary>Represents currency detailed info</summary>
    public class Currency
    {
        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// County name of the currency
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Name of the currency
        /// </summary>
        public string CurrencyName { get; set; }
        /// <summary>
        /// Exchange rate of the currency in CZK
        /// </summary>
        public string Rate { get; set; }
        /// <summary>
        /// Amount of money to be exchanged
        /// </summary>
        public string Amount { get; set; }
    }
}
