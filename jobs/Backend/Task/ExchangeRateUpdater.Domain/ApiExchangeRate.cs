namespace ExchangeRateUpdater.Domain
{
    public record ApiExchangeRate(string CurrencyCode, decimal Rate, int Amount);
}
