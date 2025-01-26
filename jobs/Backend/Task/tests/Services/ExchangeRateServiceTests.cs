using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Options;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ExchangeRateUpdater.Tests.Services
{
    public class ExchangeRateServiceTests
    {
        private readonly Mock<IExchangeRateProvider> _providerMock;
        private readonly Mock<ILogger<ExchangeRateService>> _loggerMock;
        private readonly Mock<IHostApplicationLifetime> _appLifetimeMock;
        private readonly ExchangeRateOptions _options;

        public ExchangeRateServiceTests()
        {
            _providerMock = new Mock<IExchangeRateProvider>();
            _loggerMock = new Mock<ILogger<ExchangeRateService>>();
            _appLifetimeMock = new Mock<IHostApplicationLifetime>();
            _options = new ExchangeRateOptions
            {
                CurrenciesToWatch = new[] { "USD", "EUR", "GBP" },
                BaseUrl = "https://test.com",
                HttpClient = null,
                BaseCurrency = "CZK",
                BackupFilePath = "backup.txt"
            };
        }

        [Fact]
        public async Task StartAsync_WhenSuccessful_ShouldLogRatesAndStopApplication()
        {
            // Arrange
            var expectedRates = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), 20.5m),
                new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 25.2m)
            };

            _providerMock.Setup(p => p.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>()))
                        .ReturnsAsync(expectedRates);

            var service = CreateService();

            // Act
            await service.StartAsync(CancellationToken.None);

            // Assert
            _providerMock.Verify(p => p.GetExchangeRatesAsync(It.Is<IEnumerable<Currency>>(
                currencies => currencies.Any(c => c.Code == "USD") &&
                            currencies.Any(c => c.Code == "EUR") &&
                            currencies.Any(c => c.Code == "GBP"))), Times.Once);

            _appLifetimeMock.Verify(a => a.StopApplication(), Times.Once);
            
            // Verificar que se registraron los logs
            VerifyLogInformation("Successfully retrieved {Count} exchange rates", It.IsAny<object[]>());
            foreach (var rate in expectedRates)
            {
                VerifyLogInformation("Exchange rate: {Rate}", It.Is<object[]>(args => args[0].Equals(rate)));
            }
        }

        [Fact]
        public async Task StartAsync_WhenProviderThrows_ShouldLogErrorAndRethrow()
        {
            // Arrange
            var expectedException = new Exception("Test error");
            _providerMock.Setup(p => p.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>()))
                        .ThrowsAsync(expectedException);

            var service = CreateService();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => service.StartAsync(CancellationToken.None));
            Assert.Same(expectedException, exception);

            VerifyLogError("Could not retrieve exchange rates", expectedException);
            _appLifetimeMock.Verify(a => a.StopApplication(), Times.Never);
        }

        [Fact]
        public async Task StopAsync_ShouldReturnCompletedTask()
        {
            // Arrange
            var service = CreateService();

            // Act
            var result =  service.StopAsync(CancellationToken.None);

            // Assert
            Assert.Same(Task.CompletedTask, result);
        }

        private ExchangeRateService CreateService()
        {
            var optionsMock = new Mock<IOptions<ExchangeRateOptions>>();
            optionsMock.Setup(x => x.Value).Returns(_options);

            return new ExchangeRateService(
                _providerMock.Object,
                _loggerMock.Object,
                optionsMock.Object,
                _appLifetimeMock.Object);
        }

        private void VerifyLogInformation(string message, object args)
        {
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.AtLeastOnce);
        }

        private void VerifyLogError(string message, Exception exception)
        {
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    exception,
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }
    }
} 