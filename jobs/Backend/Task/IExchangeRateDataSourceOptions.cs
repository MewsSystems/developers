namespace ExchangeRateUpdater
{
    public interface IExchangeRateDataSourceOptions
    {
        string DailyRatesUrl { get; }
        string MonthlyRatesUrl { get; }
    }
}
