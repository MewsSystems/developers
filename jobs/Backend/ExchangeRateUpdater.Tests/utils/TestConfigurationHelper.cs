using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.Test.utils
{
    public static class TestConfigurationHelper
    {
        public static IConfiguration BuildConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "ExchangeRateProvider:Url", "https://api.cnb.cz/cnbapi/exrates/daily?date=2024-06-21&lang=EN" }
                })
                .Build();

            return config;
        }
    }
}
