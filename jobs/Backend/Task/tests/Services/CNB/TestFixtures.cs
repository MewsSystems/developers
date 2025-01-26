using ExchangeRateUpdater.Options;

namespace ExchangeRateUpdater.Tests.Services.CNB
{
    public static class TestFixtures
    {
        public static ExchangeRateOptions CreateValidOptions() => new()
        {
            BaseUrl = "http://test.com/rates",
            BaseCurrency = "CZK",
            BackupFilePath = "test-backup.txt",
            CurrenciesToWatch = new[] { "USD", "EUR", "GBP" },
            HttpClient = new HttpClientOptions
            {
                TimeoutSeconds = 30,
                RetryCount = 3,
                RetryWaitSeconds = 2
            }
        };

        public static ExchangeRateOptions CreateMinimalOptions() => new()
        {
            BaseUrl = "http://test.com/rates",
            BaseCurrency = "CZK",
            CurrenciesToWatch = new[]
            {
                "USD"
            },
            HttpClient = null,
            BackupFilePath = null
        };
    }
} 