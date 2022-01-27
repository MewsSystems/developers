using System.Collections.Generic;

namespace ExchangeRateUpdater.Configuration
{
    public class ApplicationConfiguration
    {
        /// <summary>
        /// List of configured rate providers
        /// </summary>
        public IEnumerable<RateProviderConfiguration> RateProviders { get; set; }
    }
}
