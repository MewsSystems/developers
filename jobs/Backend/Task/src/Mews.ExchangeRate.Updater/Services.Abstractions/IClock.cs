using System;
using System.Collections.Generic;
using System.Text;

namespace Mews.ExchangeRate.Updater.Services.Abstractions
{
    /// <summary>
    /// This interface holds the logic to get the current time.
    /// Useful in test scenarios.
    /// </summary>
    public interface IClock
    {
        /// <summary>
        /// Gets the current UTC date and time.
        /// </summary>
        /// <value>
        /// The UTC now.
        /// </value>
        DateTime UtcNow { get; }
    }
}
