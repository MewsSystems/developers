using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Infrastructure.Providers
{
    internal class MonitorProvider : IMonitorProvider
    {
        private readonly ILogger<MonitorProvider> _logger;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public MonitorProvider(ILogger<MonitorProvider> logger)
        {
            _logger = logger;
        }

        public async Task<T> ExecuteActionAsync<T>(Func<Task<T>> method, string metricName, string className = null, string methodName = null)
        {
            _stopwatch.Start();
            var result = await method();

            _logger.LogInformation("{metricName}: {className}.{methodName} was executing (ms): {_stopwatch.ElapsedMilliseconds}", metricName, className, methodName, _stopwatch.ElapsedMilliseconds);

            return result;
        }
    }
}
