namespace ExchangeRateUpdater.Parse
{
    public record CnbCzExchangeRate(string Country, string Currency, int Amount, string Code, decimal Rate)
    {
    }
}
