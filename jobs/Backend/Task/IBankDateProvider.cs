using System;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Class providing information about dates when the bank issued daily or monthly exchange rate listings for requested dateTime.
    /// </summary>
    public interface IBankDateProvider
    {
        /// <summary>
        /// Computes date of daily listing, which was current at specified <paramref name="requestedDateTime"/>
        /// </summary>
        /// <param name="currentDateTime">Requested date time.</param>
        /// <returns>Date of daily listing.</returns>
        public DateOnly GetDailyListingBankDateForDateTime(DateTime requestedDateTime);

        /// <summary>
        /// Computes date of monthly listing, which was current at specified <paramref name="requestedDateTime"/>.
        /// </summary>
        /// <param name="requestedDateTime">Requested date time.</param>
        /// <returns>Date of monthly listing.</returns>
        public DateOnly GetMonthlyListingBankDateForDateTime(DateTime requestedDateTime);
    }

}
