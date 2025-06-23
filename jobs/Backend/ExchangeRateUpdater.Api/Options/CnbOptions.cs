namespace ExchangeRateUpdater.Api.Options
{
    /// <summary>
    /// Configuration settings for connecting to the CNB API.
    /// </summary>
    public class CnbOptions
    {
        /// <summary>
        /// The base URL of the Czech National Bank (CNB) API.  
        /// This value is retrieved from **appsettings.json**
        /// </summary>
        public required string BaseUrl { get; set; }

    }
}
