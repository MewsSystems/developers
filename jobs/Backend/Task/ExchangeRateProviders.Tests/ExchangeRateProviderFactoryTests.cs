using ExchangeRateProviders.Core;
using ExchangeRateProviders.Czk;
using ExchangeRateProviders.Czk.Config;
using ExchangeRateProviders.Tests.TestHelpers;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace ExchangeRateProviders.Tests;

[TestFixture]
public class ExchangeRateProviderFactoryTests
{
    [Test]
    public void GetProvider_UnknownCurrency_Throws()
    {
        // Arrange
        var logger = Substitute.For<ILogger<ExchangeRateProviderFactory>>();
        var factory = new ExchangeRateProviderFactory(Array.Empty<IExchangeRateProvider>(), logger);

        // Act & Assert
        Assert.Multiple(() =>
        {
            Assert.Throws<InvalidOperationException>(() => factory.GetProvider("USD"));
            logger.VerifyLogError(1, "No exchange rate provider registered for currency USD");
        });
    }

    [Test]
    public void GetProvider_NullCurrency_Throws()
    {
        // Arrange
        var logger = Substitute.For<ILogger<ExchangeRateProviderFactory>>();
        var factory = new ExchangeRateProviderFactory(Array.Empty<IExchangeRateProvider>(), logger);

        // Act & Assert
        Assert.Multiple(() =>
        {
            Assert.Throws<ArgumentNullException>(() => factory.GetProvider(null!));
            logger.VerifyLogError(1, "Attempted to get provider with null currency code.");
        });
    }

    [Test]
    public void GetProvider_KnownCurrency_ReturnsProvider()
    {
        // Arrange
        var providerLogger = Substitute.For<ILogger<CzkExchangeRateProvider>>();
        var dataProvider = Substitute.For<IExchangeRateDataProvider>();
        var provider = new CzkExchangeRateProvider(dataProvider, providerLogger);
        var factoryLogger = Substitute.For<ILogger<ExchangeRateProviderFactory>>();
        var factory = new ExchangeRateProviderFactory(new[] { provider }, factoryLogger);

        // Act
        var resolved = factory.GetProvider(Constants.ExchangeRateProviderCurrencyCode);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(resolved, Is.SameAs(provider));
            factoryLogger.VerifyLogDebug(1, $"Resolved exchange rate provider for currency {Constants.ExchangeRateProviderCurrencyCode}");
        });
    }
}
