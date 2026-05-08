using ExchangeRateProvider.Domain.Constants;
using ExchangeRateProvider.Domain.Interfaces;
using ExchangeRateProvider.Domain.ValueObjects;
using ExchangeRateProvider.Infrastructure.Factories;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace ExchangeRateProvider.Infrastructure.Tests.Unit.Factories;

public class ExchangeRateServiceFactoryTests
{
    [Theory]
    [InlineData("CZK")]
    [InlineData("czk")]
    [InlineData("Czk")]
    public void GetService_WithCzkCurrency_ReturnsService(string currencyCode)
    {
        // Arrange
        var services = new ServiceCollection();
        var fakeService = A.Fake<IExchangeRateService>();
        services.AddKeyedSingleton(CurrencyServiceKeys.CZK, fakeService);
        var factory = new ExchangeRateServiceFactory(services.BuildServiceProvider());

        // Act
        var result = factory.GetService(new Currency(currencyCode));

        // Assert
        result.ShouldBe(fakeService);
    }

    [Theory]
    [InlineData("USD")]
    [InlineData("EUR")]
    [InlineData("GBP")]
    public void GetService_WithUnsupportedCurrency_FallsBackToCzkService(string currencyCode)
    {
        // Arrange
        var services = new ServiceCollection();
        var fakeService = A.Fake<IExchangeRateService>();
        services.AddKeyedSingleton(CurrencyServiceKeys.CZK, fakeService);
        var factory = new ExchangeRateServiceFactory(services.BuildServiceProvider());

        // Act
        var result = factory.GetService(new Currency(currencyCode));

        // Assert
        result.ShouldBe(fakeService);
    }

    [Fact]
    public void GetService_WhenServiceNotRegistered_ThrowsInvalidOperationException()
    {
        // Arrange
        var services = new ServiceCollection();
        var factory = new ExchangeRateServiceFactory(services.BuildServiceProvider());

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => factory.GetService(new Currency("CZK")));
    }
}