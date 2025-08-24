using ExchangeRateProviders.Core;
using ExchangeRateProviders.Core.Model;
using ExchangeRateProviders.Czk;
using ExchangeRateProviders.Czk.Config;
using ExchangeRateProviders.Czk.Services;
using ExchangeRateProviders.Tests.TestHelpers;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace ExchangeRateProviders.Tests.Czk;

[TestFixture]
public class CzkExchangeRateProviderTests
{
    [Test]
    public async Task GetExchangeRatesAsync_NullCurrencies_ReturnsEmpty()
    {
        // Arrange
        var dataProvider = Substitute.For<ICzkExchangeRateDataProvider>();
        var logger = Substitute.For<ILogger<CzkExchangeRateProvider>>();
        var provider = new CzkExchangeRateProvider(dataProvider, logger);

        // Act
        var result = await provider.GetExchangeRatesAsync(null!, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Empty);
            logger.VerifyLogWarning(1, "Requested currencies collection is null. Returning empty result.");
        });
    }

    [Test]
    public async Task GetExchangeRatesAsync_FiltersToRequestedCurrencies()
    {
        // Arrange
        var logger = Substitute.For<ILogger<CzkExchangeRateProvider>>();
        var dataProvider = Substitute.For<ICzkExchangeRateDataProvider>();
        var allRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 22.5m),
            new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 24.0m),
            new ExchangeRate(new Currency("JPY"), new Currency("CZK"), 0.17m)
        };
        dataProvider.GetDailyRatesAsync(Arg.Any<CancellationToken>()).Returns(allRates);
        var provider = new CzkExchangeRateProvider(dataProvider, logger);
        var requested = new[] { new Currency("USD"), new Currency("JPY") };

        // Act
        var result = (await provider.GetExchangeRatesAsync(requested, CancellationToken.None)).ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(r => r.SourceCurrency.Code == "USD"));
            Assert.That(result.Any(r => r.SourceCurrency.Code == "JPY"));
            Assert.That(result.All(r => r.TargetCurrency.Code == "CZK"));
            logger.VerifyLogDebug(1, $"Fetching exchange rates for 2 requested currencies via provider {Constants.ExchangeRateProviderCurrencyCode}.");
            logger.VerifyLogInformation(1, $"Provider {Constants.ExchangeRateProviderCurrencyCode} returned 2/3 matching rates.");
        });
    }
}
