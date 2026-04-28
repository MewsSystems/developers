namespace ExchangeRateUpdater
{
    public record CnbSettings(string SourceUrl, string TargetCurrencyCode, int RetryAttempts);
}

