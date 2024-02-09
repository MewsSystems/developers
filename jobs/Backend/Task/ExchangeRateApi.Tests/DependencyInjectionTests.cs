using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateApi.Tests
{
    public class DependencyInjectionTests
    {
        [Fact]
        public void ServiceProvider_CanResolveExchangeRateProvider_WithDependencies()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddHttpClient();
            serviceCollection.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
            serviceCollection.AddSingleton(Mock.Of<ILogger<ExchangeRateProvider>>());

            // Adding IConfiguration mock setup
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c["ExchangeRateProvider:BaseUrl"]).Returns("http://testapi.com");
            serviceCollection.AddSingleton(configurationMock.Object);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Act
            var provider = serviceProvider.GetService<IExchangeRateProvider>();

            // Assert
            Assert.NotNull(provider);
            Assert.IsType<ExchangeRateProvider>(provider);
        }
    }
}
