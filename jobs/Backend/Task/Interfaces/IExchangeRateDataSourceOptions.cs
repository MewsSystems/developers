namespace ExchangeRateUpdater.Configuration
{
    public interface IExchangeRateDataSourceOptions
    {
        string DailyRatesUrl { get; }
        string MonthlyRatesUrl { get; }
    }
}
