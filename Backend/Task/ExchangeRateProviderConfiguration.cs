using System.Configuration;

namespace ExchangeRateUpdater
{
    /// <summary>
    ///     Configuration section for <see cref="ExchangeRateProvider"/>.
    /// </summary>
    /// ReSharper disable once ClassNeverInstantiated.Global
    public class ExchangeRateProviderConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("PrimaryDailyExchangeRateSource")]
        public ExchangeRateProviderPrimaryDailySourceConfiguration PrimaryDailyExchangeRateSource
            => (ExchangeRateProviderPrimaryDailySourceConfiguration) this["PrimaryDailyExchangeRateSource"];
        
        [ConfigurationProperty("SecondaryMonthlyExchangeRateSource")]
        public ExchangeRateProviderSecondaryMonthlySourceConfiguration SecondaryMonthlyExchangeRateSource 
            => (ExchangeRateProviderSecondaryMonthlySourceConfiguration) this["SecondaryMonthlyExchangeRateSource"];
    }
    
    /// <summary>
    ///     Configuration element of <see cref="ExchangeRateProviderConfiguration"/> for primary daily source.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ExchangeRateProviderPrimaryDailySourceConfiguration : ConfigurationElement
    {
        [ConfigurationProperty("url", IsRequired = true)]
        public string Url => this["url"] as string;
    }
    
    /// <summary>
    ///     Configuration element of <see cref="ExchangeRateProviderConfiguration"/> for secondary monthly source.
    /// </summary>
    /// ReSharper disable once ClassNeverInstantiated.Global
    public class ExchangeRateProviderSecondaryMonthlySourceConfiguration : ConfigurationElement
    {
        [ConfigurationProperty("url", IsRequired = true)]
        public string Url => this["url"] as string;
    }
}