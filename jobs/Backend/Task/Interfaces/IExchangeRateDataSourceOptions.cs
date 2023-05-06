namespace ExchangeRateUpdater.Interfaces
{
    public interface IExchangeRateDataSourceOptions
    {
        string DailyRatesUrl { get; }
        string MonthlyRatesUrl { get; }
    }
}
