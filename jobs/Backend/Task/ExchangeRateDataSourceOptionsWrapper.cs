namespace ExchangeRateUpdater
{
    public class ExchangeRateDataSourceOptionsWrapper : IExchangeRateDataSourceOptions
    {
        public string DailyRatesUrl { get; set; }
        public string MonthlyRatesUrl { get; set; }
    }
}
