using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Data class containing exchange rate listing from a specific day.
    /// </summary>
    public class ExchangeRatesListing
    {
        /// <summary>
        /// Date of exchange rate listing.
        /// </summary>
        public DateOnly ListingDate { get; }

        /// <summary>
        /// List of exchange rates available in the listing.
        /// </summary>
        public IReadOnlyList<ExchangeRate> ExchangeRates { get; }

        public ExchangeRatesListing(DateOnly listingDate, IReadOnlyList<ExchangeRate> exchangeRates) {
            ListingDate = listingDate;
            ExchangeRates = exchangeRates;
        }
    }
}
