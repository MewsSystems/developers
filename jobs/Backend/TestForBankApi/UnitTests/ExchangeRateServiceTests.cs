using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateServiceTests
    {
        protected Mock<IExchangeRateProvider> mockIExchangeRateProvider;
        protected Mock<IConfiguration> mockIConfiguration;

        ExchangeRateService _service;

        protected void RegisterTestDependencies(IServiceCollection services)
        {
            mockIExchangeRateProvider = new Mock<IExchangeRateProvider>();
            mockIExchangeRateProvider
                .Setup(p => p.GetExchangeRatesApiAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<DateTime?>()))
                .ReturnsAsync(new List<ExchangeRate> { new ExchangeRate(new Currency("USD"), new Currency("CZK"), 1m) });

            mockIExchangeRateProvider
                .Setup(p => p.GetExchangeRatesTextAsync(It.IsAny<IEnumerable<Currency>>()))
                .ReturnsAsync(new List<ExchangeRate> { new ExchangeRate(new Currency("USD"), new Currency("CZK"), 1m) });

            services.AddSingleton(mockIExchangeRateProvider.Object);

            mockIConfiguration = new Mock<IConfiguration>();
            services.AddSingleton(mockIConfiguration.Object);
        }

        [SetUp]
        public void Setup()
        {
            var serviceCollection = new ServiceCollection();
            RegisterTestDependencies(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            _service = serviceProvider.GetRequiredService<ExchangeRateService>();
        }

        [Test]
        public async Task ExecuteAsync_ShouldCallGetExchangeRatesApiAsync()
        {
            // Act
            await _service.ExecuteAsync();

            // Assert
            mockIExchangeRateProvider.Verify(p => p.GetExchangeRatesApiAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<DateTime?>()), Times.Once);
        }

        [Test]
        public async Task ExecuteAsync_ShouldCallGetExchangeRatesTextAsync()
        {
            // Act
            await _service.ExecuteAsync();

            // Assert
            mockIExchangeRateProvider.Verify(p => p.GetExchangeRatesTextAsync(It.IsAny<IEnumerable<Currency>>()), Times.Once);
        }
    }
}
