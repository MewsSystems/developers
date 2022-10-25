namespace ExchangeRateUpdater.Cnb.Dtos
{
    public record DailyExchangeRates(DateOnly Date, List<ExchangeRate> Rates);
}
