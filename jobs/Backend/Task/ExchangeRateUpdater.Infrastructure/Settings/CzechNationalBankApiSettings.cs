using System.Diagnostics.CodeAnalysis;

namespace ExchangeRateUpdater.Infrastructure.Settings
{
    [ExcludeFromCodeCoverage]
    internal class CzechNationalBankApiSettings
    {
        public string BaseUrl { get; set; }

        public int TimeoutSec { get; set; }

        public int RetryCount { get; set; }
    }
}
