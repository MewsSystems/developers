using System;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// DateTime wrapper, which enables setting up current dateTime in tests.
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Gets a DateTime object that is set to the current date and time on this computer, expressed as the local time.
        /// Wrapper over the DateTime.Now method.
        /// </summary>
        /// <returns>Current DateTime.</returns>
        public DateTime Now();
    }
}
