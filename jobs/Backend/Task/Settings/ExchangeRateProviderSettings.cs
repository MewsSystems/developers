namespace ExchangeRateUpdater.Settings
{
    public sealed class ExchangeRateProviderSettings
    {
        public string CnbUrl { get; set; } = string.Empty;
        public int TimeoutInSeconds { get; set; } = 20;
    }
}
