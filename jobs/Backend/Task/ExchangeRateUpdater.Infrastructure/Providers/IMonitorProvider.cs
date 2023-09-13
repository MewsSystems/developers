namespace ExchangeRateUpdater.Infrastructure.Providers
{
    public interface IMonitorProvider
    {
        Task<T> ExecuteActionAsync<T>(Func<Task<T>> method, string metricName, string className = null, string methodName = null);
    }
}
