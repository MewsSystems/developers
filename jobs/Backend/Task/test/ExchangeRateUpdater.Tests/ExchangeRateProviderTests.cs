using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderTests
    {
        private readonly Mock<IFxRateService> _fxRateService;
        private readonly Mock<ILogger<ExchangeRateProvider>> _logger;
        IExchangeRateProvider _exchangeRateProvider;

        public ExchangeRateProviderTests()
        {
            _fxRateService = new Mock<IFxRateService>();
            _logger = new Mock<ILogger<ExchangeRateProvider>>();
            _exchangeRateProvider = new ExchangeRateProvider(_fxRateService.Object, _logger.Object);
        }

        [Fact]
        public async Task GetExchangeRates_ReturnsTheCorrectExchangeRates_WhenValidListOfCurrenciesIsProvided()
        {
            var currencies = new List<Currency> { new("USD"), new("EUR"), new("GBP"), new("AUD"), new("RON"), new("XYZ"), new("BLA") };
            var apiFxRates = new List<FxRate>
            {
                new() { CurrencyCode = "USD", Rate = 23.546},
                new() { CurrencyCode = "EUR", Rate = 25.254},
                new() { CurrencyCode = "GBP", Rate = 30.021},
                new() { CurrencyCode = "AUD", Rate = 15.006},
                new() { CurrencyCode = "RON", Rate = 8.210},
            };
            var expectedResult = apiFxRates.Select(x => new ExchangeRate(new Currency(x.CurrencyCode), new Currency("CZK"), (decimal)x.Rate));
            _fxRateService.Setup(fx => fx.GetFxRatesAsync(It.IsAny<DateTime>(), It.IsAny<string>(), default)).ReturnsAsync(apiFxRates);

            var result = await _exchangeRateProvider.GetExchangeRatesAsync(currencies, DateTime.UtcNow);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetExchangeRates_ReturnsOnlyMathcingExchangeRates()
        {
            var currencies = new List<Currency> { new("USD"), new("EUR"), new("GBP"), new("AUD"), new("RON") };
            var apiFxRates = new List<FxRate>
            {
                new() { CurrencyCode = "USD", Rate = 23.546},
                new() { CurrencyCode = "EUR", Rate = 25.254},
                new() { CurrencyCode = "GBP", Rate = 30.021},
                new() { CurrencyCode = "CAD", Rate = 15.006},
                new() { CurrencyCode = "JPY", Rate = 8.210},
            };
            var expectedResult = new List<ExchangeRate>
            {
                new(new Currency("USD"), new Currency("CZK"), 23.546m),
                new(new Currency("EUR"), new Currency("CZK"), 25.254m),
                new(new Currency("GBP"), new Currency("CZK"), 30.021m),
            };
            _fxRateService.Setup(fx => fx.GetFxRatesAsync(It.IsAny<DateTime>(), It.IsAny<string>(), default)).ReturnsAsync(apiFxRates);

            var result = await _exchangeRateProvider.GetExchangeRatesAsync(currencies, DateTime.UtcNow);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetExchangeRates_ReturnsOnlyMathcingExchangeRatesWhenRequestedCurrenciesIsEmpty()
        {
            var currencies = new List<Currency>();
            var apiFxRates = new List<FxRate>
            {
                new() { CurrencyCode = "USD", Rate = 23.546},
                new() { CurrencyCode = "EUR", Rate = 25.254},
                new() { CurrencyCode = "GBP", Rate = 30.021},
                new() { CurrencyCode = "CAD", Rate = 15.006},
                new() { CurrencyCode = "JPY", Rate = 8.210},
            };
            
            _fxRateService.Setup(fx => fx.GetFxRatesAsync(It.IsAny<DateTime>(), It.IsAny<string>(), default)).ReturnsAsync(apiFxRates);

            var result = await _exchangeRateProvider.GetExchangeRatesAsync(currencies, DateTime.UtcNow);

            result.Should().BeEmpty();
            _logger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("The requested currencies list was empty. No exchange rates will be extracted.")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }
    }
}
