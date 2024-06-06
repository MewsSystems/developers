using System;
using System.Collections.Generic;
using static ExchangeRateUpdater.ExchangeRate.Model.ExchangeRateDataset;

namespace ExchangeRateUpdater.ExchangeRate.Model
{
    /// <summary>
    /// Represents a dataset containing exchange rate data for a specific date.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ExchangeRateDataset"/> class with the specified date and exchange rates.
    /// </remarks>
    /// <param name="date">The date of the dataset.</param>
    /// <param name="exchangeRates">The exchange rates in the dataset.</param>
    public class ExchangeRateDataset(DateOnly date, IEnumerable<ExchangeRateData> exchangeRates, Channel channel)
    {

        /// <summary>
        /// Gets or sets the date of the dataset.
        /// </summary>
        public DateOnly Date { get; set; } = date;

        /// <summary>
        /// Gets or sets the exchange rates in the dataset.
        /// </summary>
        public IEnumerable<ExchangeRateData> ExchangeRates { get; set; } = exchangeRates;

        /// <summary>
        /// Gets or sets the channel of the dataset.
        /// </summary>
        public Channel DataChannel { get; set; } = channel;

        public enum Channel
        {
            Direct = 1,
            Worker = 2
        }
    }
}
