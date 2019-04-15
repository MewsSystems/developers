namespace ExchangeRateUpdater.ServiceContracts
{
    /// <summary>
    /// Represents request to the <see cref="ExchangeRateService" />.
    /// </summary>
    public class LoadExchangeRatesRequest
    {
        /// <summary>
        /// The exchange date. The format is "dd.MM.yyyy". Leave empty to use the current date.
        /// </summary>
        public string ExchangeDate { get; set; }

        /// <summary>
        /// True when results of a given request are cached to a file.
        /// </summary>
        public bool UseCache { get; set; }
    }
}
