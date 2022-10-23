namespace ExchangeRateUpdater.Cnb.Dtos
{
    public record DailyExchangeRates(DateOnly Date, ExchangeRate[] Rates);
}
