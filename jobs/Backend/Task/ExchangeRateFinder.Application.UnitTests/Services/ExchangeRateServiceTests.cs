using ExchangeRateFinder.Domain.Services;
using ExchangeRateFinder.Infrastructure.Models;
using ExchangeRateFinder.Infrastructure.Services;
using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateFinder.Application.UnitTests.Services
{
    public class ExchangeRateServiceTests
    {
        private readonly ExchangeRateService _target;
        private readonly Mock<IExchangeRateRepository> _exchangeRateRepositoryMock;
        private readonly Mock<ICachingService<ExchangeRate>> _exchangeRateCachingMock;
        private readonly Mock<IExchangeRateCalculator> _exchangeRateCalculatorMock;
        private readonly Mock<ILogger<ExchangeRateService>> _loggerMock;

        public ExchangeRateServiceTests()
        {
            _loggerMock = new Mock<ILogger<ExchangeRateService>>();
            _exchangeRateRepositoryMock = new Mock<IExchangeRateRepository>();
            _exchangeRateCachingMock = new Mock<ICachingService<ExchangeRate>>();
            _exchangeRateCalculatorMock = new Mock<IExchangeRateCalculator>();

            _target = new ExchangeRateService(
                _exchangeRateRepositoryMock.Object,
                _exchangeRateCachingMock.Object,
                _exchangeRateCalculatorMock.Object,
                _loggerMock.Object);

        }

        [Fact]
        public async void GetExchangeRates_ReturnsNull_WhenExchangeRateIsNull()
        {
            // Arrange
            var sourceCurrencyCode = "CZK";
            var currencies = new List<string> { "USD" };
            ExchangeRate exchangeRate = null;
            _exchangeRateCachingMock.Setup(e => e.GetOrAddAsync(It.IsAny<string>(), It.IsAny<Func<Task<ExchangeRate>>>()))
                .ReturnsAsync(exchangeRate);

            // Act 
            var exchangeRates = await _target.GetExchangeRates(sourceCurrencyCode, currencies);

            // Assert
            Assert.Empty(exchangeRates);
            _exchangeRateCalculatorMock.Verify(e => e.Calculate(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public async void GetExchangeRates_ReturnsCorrectExchangeRates()
        {
            // Arrange
            var sourceCurrencyCode = "CZK";
            var currencies = new List<string> { "USD" };
            var exchangeRate = new ExchangeRate()
            {
                CountryName = "USA",
                SourceCurrencyCode = sourceCurrencyCode,
                TargetCurrencyCode = "USD",
                TargetCurrencyName = "dollar",
                Amount = 1,
                Value = 2.5m,
            };
            var caclulatedRate = 2.5m;

            _exchangeRateCachingMock.Setup(e => e.GetOrAddAsync(It.IsAny<string>(), It.IsAny<Func<Task<ExchangeRate>>>()))
                .ReturnsAsync(exchangeRate);

            _exchangeRateCalculatorMock.Setup(c => c.Calculate(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>())).Returns(
                new Domain.Entities.CalculatedExchangeRate() { SourceCurrencyCode = sourceCurrencyCode, TargetCurrencyCode = exchangeRate.TargetCurrencyCode, Rate = caclulatedRate });

            // Act 
            var exchangeRates = await _target.GetExchangeRates(sourceCurrencyCode, currencies);

            // Assert
            Assert.Single(exchangeRates);
            Assert.Equal(sourceCurrencyCode, exchangeRates.First().SourceCurrencyCode);
            Assert.Equal(exchangeRate.TargetCurrencyCode, exchangeRates.First().TargetCurrencyCode);
            Assert.Equal(caclulatedRate, exchangeRates.First().Rate);
            _exchangeRateCalculatorMock.Verify(e => e.Calculate(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }
    }
}
