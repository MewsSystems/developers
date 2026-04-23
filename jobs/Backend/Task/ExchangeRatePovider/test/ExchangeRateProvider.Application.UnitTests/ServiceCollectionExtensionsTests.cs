using ExchangeRateProvider.Application.Interfaces;
using ExchangeRateProvider.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ExchangeRateProvider.Application.UnitTests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddApplicationServices_RegistersExchangeRateProviderService_AsScoped()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplicationServices();

        // Assert
        var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IExchangeRateProviderService));
        
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(typeof(ExchangeRateProviderService), serviceDescriptor.ImplementationType);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddApplicationServices_ReturnsServiceCollection_ForMethodChaining()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddApplicationServices();

        // Assert
        Assert.Same(services, result);
    }

    [Fact]
    public void AddApplicationServices_CanResolveExchangeRateProviderService_WhenDependenciesProvided()
    {
        // Arrange
        var services = new ServiceCollection();
        var mockProvider = new Mock<IExchangeRateProvider>();
        
        services.AddSingleton(mockProvider.Object);
        services.AddApplicationServices();
        
        using var serviceProvider = services.BuildServiceProvider();

        // Act
        var service = serviceProvider.GetService<IExchangeRateProviderService>();

        // Assert
        Assert.NotNull(service);
        Assert.IsType<ExchangeRateProviderService>(service);
    }

    [Fact]
    public void AddApplicationServices_RegistersService_OnlyOnce()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplicationServices();

        // Assert
        var serviceDescriptors = services.Where(s => s.ServiceType == typeof(IExchangeRateProviderService));
        Assert.Single(serviceDescriptors);
    }

    [Fact]
    public void AddApplicationServices_CalledMultipleTimes_RegistersServiceMultipleTimes()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplicationServices();
        services.AddApplicationServices();

        // Assert
        var serviceDescriptors = services.Where(s => s.ServiceType == typeof(IExchangeRateProviderService));
        Assert.Equal(2, serviceDescriptors.Count());
        Assert.All(serviceDescriptors, descriptor =>
        {
            Assert.Equal(typeof(ExchangeRateProviderService), descriptor.ImplementationType);
            Assert.Equal(ServiceLifetime.Scoped, descriptor.Lifetime);
        });
    }

    [Fact]
    public void AddApplicationServices_WithExistingServices_PreservesExistingRegistrations()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<string>("test-service");

        // Act
        services.AddApplicationServices();

        // Assert
        Assert.Contains(services, s => s.ServiceType == typeof(string));
        Assert.Contains(services, s => s.ServiceType == typeof(IExchangeRateProviderService));
        Assert.Equal(2, services.Count);
    }

    [Fact]
    public void AddApplicationServices_CreatesNewInstancePerScope()
    {
        // Arrange
        var services = new ServiceCollection();
        var mockProvider = new Mock<IExchangeRateProvider>();
        
        services.AddSingleton(mockProvider.Object);
        services.AddApplicationServices();
        
        using var serviceProvider = services.BuildServiceProvider();

        // Act
        IExchangeRateProviderService service1;
        IExchangeRateProviderService service2;
        IExchangeRateProviderService service3;
        IExchangeRateProviderService service4;
        
        using (var scope1 = serviceProvider.CreateScope())
        {
            service1 = scope1.ServiceProvider.GetRequiredService<IExchangeRateProviderService>();
            service2 = scope1.ServiceProvider.GetRequiredService<IExchangeRateProviderService>();
        }
        
        using (var scope2 = serviceProvider.CreateScope())
        {
            service3 = scope2.ServiceProvider.GetRequiredService<IExchangeRateProviderService>();
            service4 = scope2.ServiceProvider.GetRequiredService<IExchangeRateProviderService>();
        }

        // Assert
        // Same instance within the same scope
        Assert.Same(service1, service2);
        Assert.Same(service3, service4);
        
        // Different instances across different scopes
        Assert.NotSame(service1, service3);
    }
}