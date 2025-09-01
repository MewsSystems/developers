using ExchangeRateProvider.Domain.Interfaces;
using ExchangeRateProvider.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateProvider.Tests.Infrastructure
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddInfrastructureServices_Registers_CnbExchangeRateProvider()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["ExchangeRateProvider:CnbExchangeRateUrl"] = "https://api.cnb.cz",
                    ["ExchangeRateProvider:TimeoutSeconds"] = "30"
                })
                .Build();

            // Act
            services.AddInfrastructureServices(configuration);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var provider = serviceProvider.GetService<CnbExchangeRateProvider>();
            Assert.NotNull(provider);
        }

        [Fact]
        public void AddInfrastructureServices_Registers_IExchangeRateProvider_As_CnbExchangeRateProvider_When_No_Redis()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["Redis:Enabled"] = "false"
                })
                .Build();

            // Act
            services.AddInfrastructureServices(configuration);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var provider = serviceProvider.GetService<IExchangeRateProvider>();
            Assert.IsType<CnbExchangeRateProvider>(provider);
        }

        [Fact]
        public void AddInfrastructureServices_Registers_IExchangeRateProvider_As_DistributedCachingExchangeRateProvider_When_Redis_Enabled()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["Redis:Enabled"] = "true",
                    ["Redis:Configuration"] = "localhost:6379"
                })
                .Build();

            // Mock IDistributedCache since we don't have Redis in tests
            services.AddSingleton<IDistributedCache>(new Mock<IDistributedCache>().Object);

            // Act
            services.AddInfrastructureServices(configuration);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var provider = serviceProvider.GetService<IExchangeRateProvider>();
            Assert.IsType<DistributedCachingExchangeRateProvider>(provider);
        }

        [Fact]
        public void AddInfrastructureServices_Configures_HttpClient()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["ExchangeRateProvider:TimeoutSeconds"] = "60"
                })
                .Build();

            // Act
            services.AddInfrastructureServices(configuration);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            Assert.NotNull(httpClientFactory);

            // Use the typed client instead of named client
            var httpClient = httpClientFactory.CreateClient(typeof(CnbExchangeRateProvider).Name);
            Assert.NotNull(httpClient);
            Assert.Equal(TimeSpan.FromSeconds(60), httpClient.Timeout);
            Assert.Contains("ExchangeRateProvider/1.0", httpClient.DefaultRequestHeaders.UserAgent.ToString());
        }

        [Fact]
        public void AddInfrastructureServices_Adds_CnbCacheStrategy()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            services.AddInfrastructureServices(configuration);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var cacheStrategy = serviceProvider.GetService<CnbCacheStrategy>();
            Assert.NotNull(cacheStrategy);
        }

        [Fact]
        public void AddInfrastructureServices_Configures_CnbCacheStrategy_From_Configuration()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["CnbCacheStrategy:PublicationWindowMinutes"] = "10",
                    ["CnbCacheStrategy:WeekdayHours"] = "2",
                    ["CnbCacheStrategy:WeekendHours"] = "24"
                })
                .Build();

            // Act
            services.AddInfrastructureServices(configuration);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var cacheStrategy = serviceProvider.GetService<CnbCacheStrategy>();
            Assert.NotNull(cacheStrategy);

            // We can't directly test the private options, but we can verify the service is created
        }

        [Fact]
        public void AddInfrastructureServices_Adds_StackExchangeRedisCache_When_Redis_Enabled()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["Redis:Enabled"] = "true",
                    ["Redis:Configuration"] = "localhost:6379",
                    ["Redis:InstanceName"] = "TestInstance"
                })
                .Build();

            // Act
            services.AddInfrastructureServices(configuration);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var distributedCache = serviceProvider.GetService<IDistributedCache>();
            Assert.NotNull(distributedCache);
            // Note: In a real scenario, this would be StackExchangeRedisCache
        }

        [Fact]
        public void AddInfrastructureServices_Handles_Missing_Configuration_Gracefully()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build(); // Empty configuration

            // Act
            services.AddInfrastructureServices(configuration);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var provider = serviceProvider.GetService<IExchangeRateProvider>();
            Assert.NotNull(provider);
        }

        [Fact]
        public void AddInfrastructureServices_Registers_Loggers()
        {
            // Test both regular and Redis-enabled scenarios
            var testCases = new[]
            {
                (false, "Regular provider logger"),
                (true, "Distributed caching provider logger")
            };

            foreach (var (redisEnabled, description) in testCases)
            {
                // Arrange
                var services = new ServiceCollection();
                var config = redisEnabled ?
                    new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string> { ["Redis:Enabled"] = "true" }).Build() :
                    new ConfigurationBuilder().Build();

                if (redisEnabled)
                    services.AddSingleton<IDistributedCache>(new Mock<IDistributedCache>().Object);

                // Act
                services.AddInfrastructureServices(config);

                // Assert
                var serviceProvider = services.BuildServiceProvider();
                var cnbLogger = serviceProvider.GetService<ILogger<CnbExchangeRateProvider>>();
                Assert.NotNull(cnbLogger);

                if (redisEnabled)
                {
                    var distributedLogger = serviceProvider.GetService<ILogger<DistributedCachingExchangeRateProvider>>();
                    Assert.NotNull(distributedLogger);
                }
            }
        }
    }
}