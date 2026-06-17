namespace Mews.ExchangeRateProvider.Infrastructure.Utils
{
    public class CNBClientOptions
    {
        public string CnbDailyRatesUrl { get; set; } = string.Empty;
        public TimeSpan Timeout { get; set; }
    }
}
