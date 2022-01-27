namespace ExchangeRateUpdater.Configuration
{
    public class RateProviderConfiguration
    {
        /// <summary>
        /// Name of the rate provider in configuration
        /// </summary>
        public string Name { get; set; }    

        /// <summary>
        /// URL to retrieve data in normalized form
        /// </summary>
        public string SourceUrl { get; set; }
    }
}
