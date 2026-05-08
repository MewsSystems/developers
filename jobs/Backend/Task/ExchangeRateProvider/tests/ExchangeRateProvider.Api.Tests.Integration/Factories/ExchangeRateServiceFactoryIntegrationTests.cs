using ExchangeRateProvider.Domain.ValueObjects;
using ExchangeRateProvider.Infrastructure;
using ExchangeRateProvider.Infrastructure.Factories;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace ExchangeRateProvider.Api.Tests.Integration.Factories;

public class ExchangeRateServiceFactoryIntegrationTests
{
    [Theory]
    [InlineData("CZK")]
    [InlineData("czk")]
    [InlineData("USD")]
    public void GetService_WithRealDependencies_ResolvesService(string currencyCode)
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddInfrastructureServices();

        var provider = services.BuildServiceProvider();
        var factory = new ExchangeRateServiceFactory(provider);

        // Act
        var service = factory.GetService(new Currency(currencyCode));

        // Assert
        service.ShouldNotBeNull();
    }

    [Fact]
    public void GetService_WhenNoServiceRegistered_ThrowsException()
    {
        // Arrange
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();
        var factory = new ExchangeRateServiceFactory(provider);

        // Act & Assert
        Should.Throw<InvalidOperationException>(() =>
            factory.GetService(new Currency("CZK")));
    }
}