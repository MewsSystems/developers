using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.UnitTests.Helpers;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRateUpdater.UnitTests.Infrastructure
{
    public class CzechNationalBankExchangeRateProviderTests
    {
        [Fact]
        public async Task GetExchangeRatesAsync_AvailableCzechNationalBankMockedResource_ReturnsUsdAndEurExchangeRates()
        {
            // Arrange
            var exchangeRateProvider = new CzechNationalBankExchangeRateProvider(CzechNationalBankServiceHelper.CreateResponsiveMockedCzechNationalBankService());
            var currencies = new[] { CurrenciesHelper.USD, CurrenciesHelper.EUR };

            // Act
            var rates = await exchangeRateProvider.GetExchangeRatesAsync(currencies);

            // Assert
            rates.Should().HaveCount(2);

            rates.Should()
                .Contain(exchangeRate => exchangeRate.SourceCurrency.Code == CurrenciesHelper.USD.Code
                                         && exchangeRate.TargetCurrency.Code == Currency.DEFAULT_CURRENCY.Code
                                         && exchangeRate.Value == CzechNationalBankServiceHelper.USD_RATE);
            rates.Should()
                .Contain(exchangeRate => exchangeRate.SourceCurrency.Code == CurrenciesHelper.EUR.Code
                                         && exchangeRate.TargetCurrency.Code == Currency.DEFAULT_CURRENCY.Code
                                         && exchangeRate.Value == CzechNationalBankServiceHelper.EUR_RATE);
        }
    }
}
