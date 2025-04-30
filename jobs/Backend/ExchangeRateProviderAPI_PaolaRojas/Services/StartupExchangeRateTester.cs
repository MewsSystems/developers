using ExchangeRateProviderAPI_PaolaRojas.Models;

namespace ExchangeRateProviderAPI_PaolaRojas.Services
{
    /// <summary>
    /// A hosted service that runs once at application startup to test the exchange rate service.
    /// It simulates a request for a predefined list of currencies and logs the result to verify
    /// that exchange rate data is being fetched and parsed correctly.
    /// </summary>
    public class StartupExchangeRateTester : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<StartupExchangeRateTester> _logger;

        public StartupExchangeRateTester(
            IServiceProvider serviceProvider,
            ILogger<StartupExchangeRateTester> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var exchangeRateService = scope.ServiceProvider.GetRequiredService<IExchangeRateService>();

            var testCurrencies = new[]
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("CZK"),
                new Currency("JPY"),
                new Currency("KES"),
                new Currency("RUB"),
                new Currency("THB"),
                new Currency("TRY"),
                new Currency("XYZ")
            };

            try
            {
                _logger.LogInformation("Testing exchange rate service at startup...");

                var result = await exchangeRateService.GetExchangeRatesAsync(testCurrencies);

                var count = result.ExchangeRates?.Count() ?? 0;
                _logger.LogInformation("Retrieved {Count} exchange rates:", count);

                foreach (var rate in result.ExchangeRates ?? [])
                {
                    _logger.LogInformation(rate.ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve exchange rates at startup.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
