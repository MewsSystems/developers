namespace ExchangeRateUpdater
{
    /// <summary>
    /// Parser for the exchange rates listing string.
    /// </summary>
    internal interface IExchangeRatesListingParser
    {
        /// <summary>
        /// Parses provided exchange rates listing string.
        /// </summary>
        /// <param name="exchangeRatesListingString">String containing the exchange rates listing to be parsed.</param>
        /// <returns>Exchange rates listing.</returns>
        public ExchangeRatesListing ParseExchangeRatesListingString(string exchangeRatesListingString);
    }
}
