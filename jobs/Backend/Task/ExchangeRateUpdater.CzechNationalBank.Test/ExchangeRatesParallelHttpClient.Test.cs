using Moq;
using ExchangeRateUpdater.Lib.Shared;
using ExchangeRateUpdater.Lib.v1CzechNationalBank.ExchangeRateProvider;

namespace ExchangeRateUpdate.CzechNationalBank.Test
{
    [TestFixture]
    public class ExchangeRatesParallelHttpClientTests
    {
        private Mock<IExchangeRateHttpClient> _mockClient;
        private IExchangeRateProviderSettings _settings;
        private IFixedWindowRateLimiter _rateLimiter;
        private ExchangeRatesParallelHttpClient _sut;
        private ILogger _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new ConsoleLogger(new ConsoleLoggerSettings());
            _settings = new ExchangeRateProviderSettings(
                sourceUrl: "test",
                timeoutSeconds: 5,
                maxThreads: 1,
                rateLimitCount: 5,
                rateLimitDuration: 1,
                precision: 4
            );

            _rateLimiter = new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions
            {
                PermitLimit = _settings.RateLimitCount,
                Window = TimeSpan.FromSeconds(_settings.RateLimitCount),
                AutoReplenishment = true
            });


            _mockClient = new Mock<IExchangeRateHttpClient>();

            _sut = new ExchangeRatesParallelHttpClient(
                _settings,
                _mockClient.Object,
                _logger,
                _rateLimiter
            );
        }


        [Test]
        public async Task GetExchangeRatesAsync_FiltersOutMissingRates()
        {
            var mockExchangeValues = new Dictionary<Currency, ProviderExchangeRate>()
            {
                [new Currency("JPY")] = new ProviderExchangeRate(new Currency("JPY"), 0m, 1) // this one should be omitted because of the zero value
            };

            // Arrange
            var currencies = new List<Currency>
            {
                new Currency("MISSING"),
                new Currency("JPY") // has a zero value so cant be processed
            };

            _mockClient.Setup(client => client.GetCurrentExchangeRateAsync(It.IsAny<Currency>()))
                .ReturnsAsync((Currency currency) => mockExchangeValues.GetValueOrDefault(currency, null));

            // Act
            var result = await _sut.GetExchangeRatesAsync(currencies);

            // Assert
            Assert.AreEqual(1, result.Count()); // JPY rate is 0, so no valid rates should be returned
        }

    }
}
