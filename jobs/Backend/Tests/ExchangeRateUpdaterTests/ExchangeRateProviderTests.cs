using ExchangeRateUpdater.Domain;
using FluentAssertions;
using Moq;

namespace ExchangeRateUpdaterTests
{
    public class ExchangeRateProviderTests
    {
        private const string ExpectedErrorMessage = "Network error";
        private readonly Mock<IExchangeRateFetcher> mockFetcher = new();
        private readonly ExchangeRateProvider sut;
        public ExchangeRateProviderTests()
        {
            sut = new ExchangeRateProvider(mockFetcher.Object);
            mockFetcher.Reset();
        }

        [Fact]
        public async Task GivenCurrencies_WhenAllRatesAreAvailable_ThenReturnRates()
        {
            // Arrange
            var allRates = new List<ExchangeRate>
            {
                new(new Currency("CZK"), new Currency("USD"), 0.045m),
                new(new Currency("CZK"), new Currency("EUR"), 0.04m),
                new(new Currency("CZK"), new Currency("JPY"), 6.5m)
            };

            mockFetcher.Setup(f => f.GetExchangeRates(It.IsAny<CancellationToken>()))
                .ReturnsAsync(allRates);

            var requestedCurrencies = new[] { new Currency("USD"), new Currency("EUR") };

            // Act
            var result = await sut.GetExchangeRates(requestedCurrencies);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().Contain(r => r.TargetCurrency.Code == "USD");
            result.Should().Contain(r => r.TargetCurrency.Code == "EUR");
            result.Should().NotContain(r => r.TargetCurrency.Code == "JPY");
        }

        [Fact]
        public async Task GivenCurrencies_WhenRateIsNotAvailable_ThenReturnEmptyRates()
        {
            // Arrange
            var allRates = new List<ExchangeRate>
            {
                new(new Currency("CZK"), new Currency("USD"), 0.045m),
            };

            mockFetcher.Setup(f => f.GetExchangeRates(It.IsAny<CancellationToken>()))
                .ReturnsAsync(allRates);

            var requestedCurrencies = new[] { new Currency("JPY") };

            // Act
            var result = await sut.GetExchangeRates(requestedCurrencies);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GivenCurrencies_WhenFetcherThrows_ThenThrowException()
        {
            // Arrange
            mockFetcher.Setup(f => f.GetExchangeRates(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Network error"));

            var requestedCurrencies = new[] { new Currency("JPY") };

            // Act
            var act = async () => await sut.GetExchangeRates(requestedCurrencies);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage(ExpectedErrorMessage);
        }
    }
}
