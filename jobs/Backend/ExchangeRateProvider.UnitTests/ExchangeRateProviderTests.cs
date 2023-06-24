using ExchangeRateProvider.Models;
using ExchangeRateProvider.Services;
using FluentAssertions;
using Moq;

namespace ExchangeRateProvider.UnitTests
{
    public class ExchangeRateProviderTests
    {
        [Fact]
        public async Task GetExchangeRatesAsync_Should_Return_Currencies_Defined_By_The_Source()
        {
            // arrange
            var targetCurrencyCode = "CZK";
            var sourceCurrencyRates = new[]
            {
                new ExchangeRate(new Currency("USD"), new Currency(targetCurrencyCode), (decimal)0.046),
                new ExchangeRate(new Currency("EUR"), new Currency(targetCurrencyCode), (decimal)0.042)
            };
            var currencies = new[]
            {
                new Currency("EUR"),
                new Currency("ALL")
            };
            var expectedResult = new[]
            {
                new ExchangeRate(new Currency("EUR"), new Currency(targetCurrencyCode), (decimal)0.042)
            };

            var exchangeRateServiceMock = new Mock<IExchangeRateService>();
            exchangeRateServiceMock.Setup(s => s.GetCurrencyExchangeRatesAsync(targetCurrencyCode)).ReturnsAsync(sourceCurrencyRates);
            var exchangeRateProvider = new Services.ExchangeRateProvider(targetCurrencyCode, exchangeRateServiceMock.Object);

            // act
            var actualResult = await exchangeRateProvider.GetExchangeRatesAsync(currencies);

            // assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}