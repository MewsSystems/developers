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
        private readonly IExchangeRateServiceSettings _settings;
        private readonly CacheHelper _cacheHelper;
        private readonly ILogger _logger;

        private const string ExchangeRateCacheKey = "DailyExchangeRates";

        public ExchangeRateProvider(ExchangeRateProviderSettings settings, ILogger logger)
        {
            _logger   = logger;
            _settings = settings;

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(logger)
                             .AddSingleton<IExchangeRateServiceSettings>(settings)
                             .AddHttpClient<IExchangeRateUpdaterService, CnbService>(httpClient => httpClient.Timeout = TimeSpan.FromSeconds(10))
                             .AddPolicyHandler(GetAsyncPolicy());

            if (settings.UseInMemoryCache)
            {
                serviceCollection.AddMemoryCache();
                serviceCollection.AddSingleton<CacheHelper>();
            }
            var serviceProvider = serviceCollection.BuildServiceProvider();
            _service     = serviceProvider.GetService<IExchangeRateUpdaterService>()!;
            _cacheHelper = serviceProvider.GetService<CacheHelper>()!;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            IEnumerable<ExchangeRate> rates = null;
            rates = _cacheHelper.GetCache<IEnumerable<ExchangeRate>>(ExchangeRateCacheKey)!;

            if (rates == null)
            {
                var currentTime = DateTime.UtcNow.ConvertTimeFromUtcWithTimezoneId(_settings.TimezoneId);
                rates = await _service.GetExchangeRatesAsync(currentTime);

                _cacheHelper.SetCache(ExchangeRateCacheKey, rates, _settings.CacheExpiryTime);
            }

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