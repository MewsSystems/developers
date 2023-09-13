using AutoFixture;
using AutoFixture.AutoNSubstitute;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Models.Enums;
using ExchangeRateUpdater.Domain.Providers;
using ExchangeRateUpdater.Infrastructure.Services;
using NSubstitute;
using Xunit;

namespace ExchangeRateUpdater.Infrastructure.UnitTests.Services
{
    public class ExchangeRateServiceTests
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoNSubstituteCustomization() { ConfigureMembers = true });
        private readonly IExchangeRateProviderFactory _exchangeRateProviderFactory;
        private readonly ExchangeRateService _subjectUnderTest;

        public ExchangeRateServiceTests()
        {
            _exchangeRateProviderFactory = Substitute.For<IExchangeRateProviderFactory>();
            _subjectUnderTest = new ExchangeRateService(_exchangeRateProviderFactory);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ShouldCreateProviderWithFactory()
        {
            // Arrange
            var currencies = _fixture.Create<IEnumerable<Currency>>();
            var targetCurrency = CurrencyCode.USD;
            var rates = _fixture.Create<IEnumerable<ExchangeRate>>();

            var provider = Substitute.For<IExchangeRateProvider>();
            provider.GetExchangeRatesAsync(currencies, targetCurrency)
                .Returns(rates);

            _exchangeRateProviderFactory.Create(targetCurrency)
                .Returns(provider);

            // Act
            await _subjectUnderTest.GetExchangeRatesAsync(currencies, targetCurrency);

            // Assert
            _exchangeRateProviderFactory.Received(1).Create(targetCurrency);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsRatesFromProvider()
        {
            // Arrange
            var currencies = _fixture.Create<IEnumerable<Currency>>();
            var targetCurrency = CurrencyCode.USD;
            var rates = _fixture.Create<IEnumerable<ExchangeRate>>();

            var provider = Substitute.For<IExchangeRateProvider>();
            provider.GetExchangeRatesAsync(currencies, targetCurrency)
                .Returns(rates);

            _exchangeRateProviderFactory.Create(targetCurrency)
                .Returns(provider);

            // Act
            var response = await _subjectUnderTest.GetExchangeRatesAsync(currencies, targetCurrency);

            // Assert
            Assert.Equal(rates, response.ExchangeRates);
        }
    }
}
