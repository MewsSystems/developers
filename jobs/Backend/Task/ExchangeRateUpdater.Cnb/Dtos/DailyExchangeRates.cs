namespace ExchangeRateUpdater
{
    public record DailyExchangeRates(DateOnly Date, ExchangeRate[] Rates);
}
