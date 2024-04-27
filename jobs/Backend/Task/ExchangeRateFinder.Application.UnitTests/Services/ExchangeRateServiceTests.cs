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
        public async void GetExchangeRates_Throws_WhenCurrenciesIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                async() => await _target.GetExchangeRates("CZK", null));
        }

        [Fact]
        public async void GetExchangeRates_Throws_WhenSourceCurrencyIsEmpty()
        {
            var currencies = new List<string> { "USD", "BGN" };
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await _target.GetExchangeRates("", currencies));
        }

        [Fact]
        public async void GetExchangeRates_Throws_WhenSourceCurrencyIsNull()
        {
            var currencies = new List<string> { "USD", "BGN" };
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await _target.GetExchangeRates(null, currencies));
        }

        [Fact]
        public async void GetExchangeRates_ReturnsNull_WhenExchangeRateIsNull()
        {
            // Arrange
            var sourceCurrency = "CZK";
            var currencies = new List<string> { "USD" };
            ExchangeRate exchangeRate = null;
            _exchangeRateCachingMock.Setup(e => e.GetOrAddAsync(It.IsAny<string>(), It.IsAny<Func<Task<ExchangeRate>>>()))
                .ReturnsAsync(exchangeRate);

            // Act 
            var exchangeRates = await _target.GetExchangeRates(sourceCurrency, currencies);

            // Assert
            Assert.Empty(exchangeRates);
            _exchangeRateCalculatorMock.Verify(e => e.Calculate(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public async void GetExchangeRates_ReturnsCorrectExchangeRates()
        {
            // Arrange
            var sourceCurrency = "CZK";
            var currencies = new List<string> { "USD" };
            var exchangeRate = new ExchangeRate()
            {
                Country = "USA",
                SourceCurrency = sourceCurrency,
                TargetCurrency = "USD",
                CurrencyCode = "USD",
                Amount = 1,
                Rate = 2.5m,
            };
            var caclulatedValue = 2.5m;

            _exchangeRateCachingMock.Setup(e => e.GetOrAddAsync(It.IsAny<string>(), It.IsAny<Func<Task<ExchangeRate>>>()))
                .ReturnsAsync(exchangeRate);

            _exchangeRateCalculatorMock.Setup(c => c.Calculate(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>())).Returns(
                new Domain.Entities.CalculatedExchangeRate() { SourceCurrency = sourceCurrency, TargetCurrency = exchangeRate.TargetCurrency, Value = caclulatedValue });

            // Act 
            var exchangeRates = await _target.GetExchangeRates(sourceCurrency, currencies);

            // Assert
            Assert.Single(exchangeRates);
            Assert.Equal(sourceCurrency, exchangeRates.First().SourceCurrency);
            Assert.Equal(exchangeRate.TargetCurrency, exchangeRates.First().TargetCurrency);
            Assert.Equal(caclulatedValue, exchangeRates.First().Value);
            _exchangeRateCalculatorMock.Verify(e => e.Calculate(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }
    }
}
