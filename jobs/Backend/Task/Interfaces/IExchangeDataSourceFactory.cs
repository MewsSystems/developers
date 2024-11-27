using ExchangeRateProvider.Models;

namespace ExchangeRateUpdater.Interfaces
{
    public interface IExchangeDataSourceFactory
    {
        public BaseExchangeDataSource CreateDataSource(ExchangeRateDataSourceType dataSourceType);
    }
}
