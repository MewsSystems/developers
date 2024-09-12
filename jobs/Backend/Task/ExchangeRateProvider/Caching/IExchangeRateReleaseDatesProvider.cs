
namespace ExchangeRateUpdater.Caching
{
    /// <summary>
    /// Provides information about expecting releasing times. 
    /// </summary>
    public interface IExchangeRateReleaseDatesProvider
    {
        /// <summary>
        /// Returns the actual release date of Exchange rates, that is valid and the current time.
        /// </summary>
        /// <returns></returns>
        DateOnly GetCurrentReleaseDate();

        /// <summary>
        /// Returns time interval to the next expected release of daily exchange rates.
        /// </summary>
        /// <returns></returns>
        TimeSpan GetTimeToNextRelease();
    }
}