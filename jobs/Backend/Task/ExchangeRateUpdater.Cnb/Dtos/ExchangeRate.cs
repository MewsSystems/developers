namespace ExchangeRateUpdater
{
    public record ExchangeRate(
        string Country, 
        string Currency,
        int Amount,
        string Code,
        decimal Rate);
}
