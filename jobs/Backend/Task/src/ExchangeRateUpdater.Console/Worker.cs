using Microsoft.Extensions.Hosting;
using ExchangeRateUpdater.Core.Providers;
using ExchangeRateUpdater.Core.Exceptions;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Core.Configuration;

namespace ExchangeRateUpdater.Console
{
    public class Worker : BackgroundService
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;
        private readonly ILogger<Worker> _logger;
        private readonly AppConfiguration _appConfiguration;

        public Worker(
            IExchangeRateProvider exchangeRateProvider,
            ILogger<Worker> logger,
            AppConfiguration appConfiguration)
        {
            _exchangeRateProvider = exchangeRateProvider;
            _logger = logger;
            _appConfiguration = appConfiguration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogDebug($"The process has been cancelled at {DateTime.UtcNow}");
                    stoppingToken.ThrowIfCancellationRequested();
                }

                try
                {
                    var currencies = _appConfiguration.AvailableCurrencies.Split(',').ToList();
                    var rates = await _exchangeRateProvider.GetExchangeRatesAsync(currencies);

                    _logger.LogDebug($"Successfully retrieved {rates.Count()} exchange rates:");
                    foreach (var rate in rates)
                    {
                        _logger.LogInformation(rate.ToString());
                    }
                }
                catch (CzechNationalBankApiException e)
                {
                    _logger.LogError($"Could not retrieve exchange rates: '{e.Message}'.", e);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error retrieving exchange rates: '{e.Message}'.", e);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"There was an unhandled error in the worker with exception: {e.Message}", e);
            }
        }
    }
}
