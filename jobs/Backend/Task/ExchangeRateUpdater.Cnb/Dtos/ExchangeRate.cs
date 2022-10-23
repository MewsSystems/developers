namespace ExchangeRateUpdater.Cnb.Dtos
{
    public record ExchangeRate(
        string Country,
        string SourceCurrencyName,
        int Amount,
        string SourceCurrencyCode,
        string TargetCurrencyCode,
        decimal Rate);
}
