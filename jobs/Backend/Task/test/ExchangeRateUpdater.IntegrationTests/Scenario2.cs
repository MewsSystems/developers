using Microsoft.Extensions.DependencyInjection;
using ExchangeRateUpdater.Features;
using ExchangeRateUpdater.Features.Configuration;
using ExchangeRateUpdater.Features.Services;
using ExchangeRateUpdater.Features.Services.ExchangeRagesDaily.V1;
using ExchangeRateUpdater.Models.Domain;
using ExchangeRateUpdater.Features.Exceptions;

namespace ExchangeRateUpdater.IntegrationTests
{
    /// <summary>
    /// GIVEN the list of currencies Empty
    /// WHEN we use ExchangeRateProvider
    /// THEN it return ExchangeUpdateRaterException
    /// </summary>
    public class Scenario2
    {
        private readonly string _baseUrl;
        private readonly ServiceCollection _serviceCollector;

        public Scenario2()
        {
            _baseUrl = "https://api.cnb.cz";
            _serviceCollector = new ServiceCollection();
            _serviceCollector.AddMemoryCache();

            _serviceCollector.AddExchangeRateUpdaterFeature(opts =>
            {
                opts.RetryOptions = RetryOptions.Default;
                opts.BaseUrl = _baseUrl;
            });

        }

        [Fact]
        public async Task Test_Scenario2()
        {
            var serviceProvider = _serviceCollector.BuildServiceProvider();

            var exchangeRateService = serviceProvider.GetRequiredService<IExchangeRateService>();
            await Assert.ThrowsAsync<ExchangeRateUpdaterException>(async () => await exchangeRateService.GetExchangeRates(Enumerable.Empty<CurrencyModel>()));


        }
    }
}