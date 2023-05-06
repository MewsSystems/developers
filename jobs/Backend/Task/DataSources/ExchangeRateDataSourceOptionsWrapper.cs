using ExchangeRateUpdater.Configuration;

namespace ExchangeRateUpdater.DataSources
{
    public class ExchangeRateDataSourceOptionsWrapper : IExchangeRateDataSourceOptions
    {
        public string DailyRatesUrl { get; set; }
        public string MonthlyRatesUrl { get; set; }
    }
}
