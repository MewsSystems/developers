using System;
using System.Collections.Generic;

namespace Mews.ExchangeRate.Storage.Abstractions.Models
{
    /// <summary>
    /// This class holds the logic to get the storage state.
    /// </summary>
    public class StorageState
    {
        /// <summary>
        /// Gets or sets the source of the data in storage.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the exchange rate keys.
        /// </summary>
        /// <value>
        /// The exchange rate keys.
        /// </value>
        public IEnumerable<string> ExchangeRateKeys { get; set; }

        /// <summary>
        /// Gets or sets the UTC time that the storage was synchronized.
        /// </summary>
        /// <value>
        /// The last update time stamp.
        /// </value>
        public DateTimeOffset LastUpdateTimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the last update status.
        /// </summary>
        /// <value>
        /// The last update status.
        /// </value>
        public UpdateStatus LastUpdateStatus { get; set; }

        /// <summary>
        /// This enum represents the status of an storage update.
        /// </summary>
        public enum UpdateStatus
        {
            Success,
            Failure,
            Unknown
        }
    }


    /// <summary>
    /// Gets the storage state of an endpoint data asynchronously.
    /// </summary>
    /// <param name="endpoint">The external endpoint to fetch data.</param>
    /// <returns></returns>
    //Task<StorageState> GetStorageStateAsync(string source);
}
