using Microsoft.Extensions.DependencyInjection;
using ExchangeRateUpdater.Features;
using ExchangeRateUpdater.Features.Configuration;
using ExchangeRateUpdater.Features.Services;
using ExchangeRateUpdater.Features.Services.ExchangeRagesDaily.V1;

namespace ExchangeRateUpdater.IntegrationTests
{

    /// <summary>
    /// GIVEN the list of currencies USD, EUR, CZK, JPY, KES, RUB, THB, TRY, XYZ
    /// WHEN we use ExchangeRateProvider
    /// THEN it return as valid currencies
    /// EUR, JPY, THB, TRY, USD
    /// </summary>
    public class Scenario1
    {
        private readonly string _baseUrl;
        private readonly ServiceCollection _serviceCollector;

        private static IEnumerable<CurrencyModel> currenciesModel = new[]
        {
            new CurrencyModel("USD"),
            new CurrencyModel("EUR"),
            new CurrencyModel("CZK"),
            new CurrencyModel("JPY"),
            new CurrencyModel("KES"),
            new CurrencyModel("RUB"),
            new CurrencyModel("THB"),
            new CurrencyModel("TRY"),
            new CurrencyModel("XYZ")
        };


        public Scenario1()
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

        [Theory]
        [InlineData("USD", true)]
        [InlineData("JPY", true)]
        [InlineData("THB", true)]
        [InlineData("TRY", true)]
        [InlineData("EUR", true)]
        [InlineData("CZK", false)]
        [InlineData("RUB", false)]
        [InlineData("XYZ", false)]
        public async Task Test_Scenario1(
            string currency,
            bool isValid)
        {

            var serviceProvider = _serviceCollector.BuildServiceProvider();

            var exchangeRateService = serviceProvider.GetRequiredService<IExchangeRateService>();
            var actual = await exchangeRateService.GetExchangeRates(currenciesModel);

            actual = actual.ToList();

            Assert.Equal(isValid, actual.Any(x=> x.TargetCurrency.Code == currency));
        }
    }
}