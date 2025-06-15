using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.Tests.TestUtils;

public static class SetupUtils
{
    public static IConfiguration GetTestConfig()
    {
        var configurationData = new Dictionary<string, string>
        {
            { "PROVIDER", "CNB" },
            { "EXCHANGES:CNB_BASE_URL", "http://localhost" },
            { "EXCHANGES:CNB_TARGET_CURRENCY", "CZK" },
            { "EXCHANGES:CNB_DAILY_EXRATE_URL", "http://localhost" }
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(configurationData)
            .Build();
    }
}
