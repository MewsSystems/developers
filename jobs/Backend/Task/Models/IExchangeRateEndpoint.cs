namespace ExchangeRateUpdater.Models
{
    /// <summary>
    /// Exchange rate endpoint description
    /// </summary>
    public interface IExchangeRateEndpoint
    {
        /// <summary>
        /// Url address of data endpoint
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Url Parameters
        /// To specify current year/month/day use a following macros %YEAR%, %MONTH%, %DAY%
        /// </summary>
        /// <example>
        /// "year=%YEAR%&month=%MONTH%"
        /// </example>
        string Parameters { get; }

        /// <summary>
        /// Endpoint name (used for logging)
        /// </summary>
        string Name { get; }
    }
}
