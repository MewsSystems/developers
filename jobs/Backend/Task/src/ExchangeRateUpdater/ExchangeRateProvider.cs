using ExchangeRateUpdater.Contracts;
using ExchangeRateUpdater.Helpers;
using ExchangeRateUpdater.Models.Entities;
using ExchangeRateUpdater.Service.Cnb;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Serilog;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly IExchangeRateUpdaterService _service;
        private readonly ILogger _logger;
        private readonly string _timezone;

        public ExchangeRateProvider(ExchangeRateProviderSettings settings, ILogger logger)
        {
            _timezone = settings.TimezoneId;
            _logger   = logger;

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(logger)
                             .AddSingleton<IExchangeRateServiceSettings>(settings)
                             .AddHttpClient<IExchangeRateUpdaterService, CnbService>(httpClient => httpClient.Timeout = TimeSpan.FromSeconds(10))
                             .AddPolicyHandler(GetAsyncPolicy());

            var serviceProvider = serviceCollection.BuildServiceProvider();
            _service = serviceProvider.GetService<IExchangeRateUpdaterService>()!;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var currentTime = DateTime.UtcNow.WithTimezone(_timezone);
            var rates = await _service.GetExchangeRatesAsync(currentTime);

            var filteredRates = rates.Where(r => currencies.Any(c => c.Code == r.SourceCurrency.Code));

            return filteredRates;
        }

        private IAsyncPolicy<HttpResponseMessage> GetAsyncPolicy()
        {
            return new PolicyCreator(new []
            {
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(3), 
            }, _logger).CreateAsyncRetryPolicy();
        }
    }
}