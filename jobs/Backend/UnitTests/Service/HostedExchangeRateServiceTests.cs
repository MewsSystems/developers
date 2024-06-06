using ExchangeRateUpdater.ExchangeRate.Constant;
using ExchangeRateUpdater.ExchangeRate.Controller.Model;
using ExchangeRateUpdater.ExchangeRate.Exception;
using ExchangeRateUpdater.ExchangeRate.Factory;
using ExchangeRateUpdater.ExchangeRate.Model;
using ExchangeRateUpdater.ExchangeRate.Provider;
using ExchangeRateUpdater.ExchangeRate.Repository;
using ExchangeRateUpdater.ExchangeRate.Service;
using Hangfire;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Tests
{
    public class HostedExchangeRateServiceTests
    {
        [Fact]
        public async Task GetDailyExchangeRates_ReturnsCachedData_WhenDataExistsForRequestedDate()
        {
            // Arrange
            var repositoryMock = new Mock<IExchangeRateRepository>();
            var exchangeRateProviderFactoryMock = new Mock<IExchangeRateProviderFactory>();
            var loggerMock = new Mock<ILogger<HostedExchangeRateService>>();
            var recurringJobManagerMock = new Mock<IRecurringJobManager>();
            var cachedData = new ExchangeRateDataset(
                new DateOnly(2024, 6, 4), // Date matches requested date
                [new ExchangeRateData("Dollar", "USD", "USA", 1.2m)], 
                ExchangeRateDataset.Channel.Direct
            );

            repositoryMock.Setup(repo => repo.GetExchangeRates(It.IsAny<string>())).ReturnsAsync(cachedData);

            var service = new HostedExchangeRateService(
                exchangeRateProviderFactoryMock.Object,
                repositoryMock.Object,
                loggerMock.Object,
                recurringJobManagerMock.Object
            );

            var request = new FetchDailyExchangeRateRequestInternal(
                new Currency("USD"), // Base currency
                new DateOnly(2024, 6, 4), // Requested date
                Language.EN, // Language
                [new Currency("USD")]
            );

            // Act
            var response = await service.GetDailyExchangeRatesAsync(request, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.NotEmpty(response.ExchangeRates);
            repositoryMock.Verify(repo => repo.GetExchangeRates(It.IsAny<string>()), Times.Once); // Repository method called once

            // Additional assertion: Verify that the recurring job manager was not called
            recurringJobManagerMock.VerifyNoOtherCalls(); // Ensure no other methods were called on the recurring job manager
        }

        [Fact]
        public async Task GetDailyExchangeRates_ReturnsFreshData_WhenNoCachedDataExists()
        {
            // Arrange
            var repositoryMock = new Mock<IExchangeRateRepository>();
            var exchangeRateProviderFactoryMock = new Mock<IExchangeRateProviderFactory>();
            var loggerMock = new Mock<ILogger<HostedExchangeRateService>>();
            var recurringJobManagerMock = new Mock<IRecurringJobManager>();

            var service = new HostedExchangeRateService(
                exchangeRateProviderFactoryMock.Object,
                repositoryMock.Object,
                loggerMock.Object,
                recurringJobManagerMock.Object
            );

            var request = new FetchDailyExchangeRateRequestInternal(
                new Currency("USD"),
                new DateOnly(2024, 6, 4),
                Language.EN,
                [new Currency("EUR")]
            );

            repositoryMock.Setup(repo => repo.GetExchangeRates(It.IsAny<string>())).ReturnsAsync((ExchangeRateDataset)null); // No cached data

            var freshData = new List<ExchangeRateData> { new("Euro", "EUR", "Europe", 0.85m) };

            exchangeRateProviderFactoryMock.Setup(factory => factory.GetProvider(request.BaseCurrency)).Returns(Mock.Of<IExchangeRateProvider>());
            Mock.Get(exchangeRateProviderFactoryMock.Object.GetProvider(request.BaseCurrency)).Setup(provider => provider.GetDailyExchangeRates(request.Date, request.Language, CancellationToken.None)).ReturnsAsync(freshData);

            // Act
            var response = await service.GetDailyExchangeRatesAsync(request, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.NotEmpty(response.ExchangeRates); // Fresh data returned
            repositoryMock.Verify(repo => repo.GetExchangeRates(It.IsAny<string>()), Times.Once); // Repository method called once
            exchangeRateProviderFactoryMock.Verify(factory => factory.GetProvider(request.BaseCurrency), Times.AtLeastOnce); // Provider factory method called once
            Mock.Get(exchangeRateProviderFactoryMock.Object.GetProvider(request.BaseCurrency)).Verify(provider => provider.GetDailyExchangeRates(request.Date, request.Language, CancellationToken.None), Times.Once); // Provider method called once
            repositoryMock.Verify(repo => repo.SaveExchangeRates(It.IsAny<string>(), It.IsAny<ExchangeRateDataset>()), Times.Once); // Repository Save method called once
        }

        [Fact]
        public async Task GetDailyExchangeRates_ThrowsException_WhenNoDataFoundForSpecifiedDate()
        {
            // Arrange
            var repositoryMock = new Mock<IExchangeRateRepository>();
            var exchangeRateProviderFactoryMock = new Mock<IExchangeRateProviderFactory>();
            var loggerMock = new Mock<ILogger<HostedExchangeRateService>>();
            var recurringJobManagerMock = new Mock<IRecurringJobManager>();

            var service = new HostedExchangeRateService(
                exchangeRateProviderFactoryMock.Object,
                repositoryMock.Object,
                loggerMock.Object,
                recurringJobManagerMock.Object
            );

            var request = new FetchDailyExchangeRateRequestInternal(
                new Currency("USD"),
                new DateOnly(2024, 6, 4),
                Language.CZ,
                [new("GBP")]
            );

            repositoryMock.Setup(repo => repo.GetExchangeRates(It.IsAny<string>())).ReturnsAsync((ExchangeRateDataset)null); // No cached data

            exchangeRateProviderFactoryMock.Setup(factory => factory.GetProvider(request.BaseCurrency)).Returns(Mock.Of<IExchangeRateProvider>());
            Mock.Get(exchangeRateProviderFactoryMock.Object.GetProvider(request.BaseCurrency)).Setup(provider => provider.GetDailyExchangeRates(request.Date, request.Language, CancellationToken.None)).ReturnsAsync([]);

            // Act & Assert
            await Assert.ThrowsAsync<ExchangeRateUpdaterException>(() => service.GetDailyExchangeRatesAsync(request, CancellationToken.None));
        }

        [Fact]
        public async Task GetDailyExchangeRates_ReturnsCachedData_WhenCacheIsUpToDate()
        {
            // Arrange
            var repositoryMock = new Mock<IExchangeRateRepository>();
            var exchangeRateProviderFactoryMock = new Mock<IExchangeRateProviderFactory>();
            var loggerMock = new Mock<ILogger<HostedExchangeRateService>>();
            var recurringJobManagerMock = new Mock<IRecurringJobManager>();

            var service = new HostedExchangeRateService(
                exchangeRateProviderFactoryMock.Object,
                repositoryMock.Object,
                loggerMock.Object,
                recurringJobManagerMock.Object
            );

            var request = new FetchDailyExchangeRateRequestInternal(
                new Currency("USD"), // Base currency
                new DateOnly(2024, 6, 4), // Requested date
                Language.EN, // Language
                [new ("EUR")]
            );

            var cachedData = new ExchangeRateDataset(request.Date, [new ("Euro", "EUR", "Europe", 1.2m)], ExchangeRateDataset.Channel.Direct); // Cached data is up-to-date
            repositoryMock.Setup(repo => repo.GetExchangeRates(It.IsAny<string>())).ReturnsAsync(cachedData);

            // Act
            var response = await service.GetDailyExchangeRatesAsync(request, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.NotEmpty(response.ExchangeRates); // Cached data returned
            repositoryMock.Verify(repo => repo.GetExchangeRates(It.IsAny<string>()), Times.Once); // Repository method called once
            exchangeRateProviderFactoryMock.Verify(factory => factory.GetProvider(It.IsAny<Currency>()), Times.Never); // Provider factory method not called
            repositoryMock.Verify(repo => repo.SaveExchangeRates(It.IsAny<string>(), It.IsAny<ExchangeRateDataset>()), Times.Never); // Repository Save method not called
        }

        [Fact]
        public async Task GetDailyExchangeRates_ThrowsException_WhenProviderFails()
        {
            // Arrange
            var repositoryMock = new Mock<IExchangeRateRepository>();
            var exchangeRateProviderFactoryMock = new Mock<IExchangeRateProviderFactory>();
            var loggerMock = new Mock<ILogger<HostedExchangeRateService>>();
            var recurringJobManagerMock = new Mock<IRecurringJobManager>();

            var service = new HostedExchangeRateService(
                exchangeRateProviderFactoryMock.Object,
                repositoryMock.Object,
                loggerMock.Object,
                recurringJobManagerMock.Object
            );

            var request = new FetchDailyExchangeRateRequestInternal(
                new Currency("USD"),
                new DateOnly(2024, 6, 4),
                Language.EN,
                [new("GBP")]
            );

            repositoryMock.Setup(repo => repo.GetExchangeRates(It.IsAny<string>())).ReturnsAsync((ExchangeRateDataset)null); // No cached data

            exchangeRateProviderFactoryMock.Setup(factory => factory.GetProvider(request.BaseCurrency)).Returns(Mock.Of<IExchangeRateProvider>());
            Mock.Get(exchangeRateProviderFactoryMock.Object.GetProvider(request.BaseCurrency)).Setup(provider => provider.GetDailyExchangeRates(request.Date, request.Language, CancellationToken.None)).Throws(new Exception("Provider failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.GetDailyExchangeRatesAsync(request, CancellationToken.None));
        }

        [Fact]
        public async Task UpdateDailyExchangeRates_ThrowsException_WhenErrorOccurs()
        {
            // Arrange
            var repositoryMock = new Mock<IExchangeRateRepository>();
            var exchangeRateProviderFactoryMock = new Mock<IExchangeRateProviderFactory>();
            var loggerMock = new Mock<ILogger<HostedExchangeRateService>>();
            var recurringJobManagerMock = new Mock<IRecurringJobManager>();

            var service = new HostedExchangeRateService(
                exchangeRateProviderFactoryMock.Object,
                repositoryMock.Object,
                loggerMock.Object,
                recurringJobManagerMock.Object
            );

            var currency = new Currency("USD");
            var date = new DateOnly(2024, 6, 4);
            var cancellationToken = new CancellationToken();

            // Simulate error by throwing an exception when retrieving the provider
            exchangeRateProviderFactoryMock.Setup(factory => factory.GetProvider(currency)).Throws(new Exception("Error fetching data"));

            // Act & Assert
            await Assert.ThrowsAsync<ExchangeRateUpdaterException>(() => service.UpdateDailyExchangeRatesAsync(currency, date, cancellationToken));
        }

        [Fact]
        public async Task GetDailyExchangeRates_SavesProviderDataInRepository_WhenFreshDataFetched()
        {
            // Arrange
            var repositoryMock = new Mock<IExchangeRateRepository>();
            var exchangeRateProviderFactoryMock = new Mock<IExchangeRateProviderFactory>();
            var loggerMock = new Mock<ILogger<HostedExchangeRateService>>();
            var recurringJobManagerMock = new Mock<IRecurringJobManager>();

            var service = new HostedExchangeRateService(
                exchangeRateProviderFactoryMock.Object,
                repositoryMock.Object,
                loggerMock.Object,
                recurringJobManagerMock.Object
            );

            var request = new FetchDailyExchangeRateRequestInternal(
                new Currency("USD"),
                new DateOnly(2024, 6, 4),
                Language.EN,
                [new Currency("EUR"), new Currency("GBP")]
            );

            var datasetKey = "USD_EN_20240604";
            var freshData = new List<ExchangeRateData>
            {
                new("Euro", "EUR", "Europe", 0.85M),
                new("British Pound", "GBP", "UK", 0.75M)
            };

            repositoryMock.Setup(repo => repo.GetExchangeRates(datasetKey)).ReturnsAsync((ExchangeRateDataset)null); // No cached data

            var providerMock = new Mock<IExchangeRateProvider>();
            exchangeRateProviderFactoryMock.Setup(factory => factory.GetProvider(request.BaseCurrency)).Returns(providerMock.Object);
            providerMock.Setup(provider => provider.GetDailyExchangeRates(request.Date, request.Language, CancellationToken.None)).ReturnsAsync(freshData);

            // Act
            var result = await service.GetDailyExchangeRatesAsync(request, CancellationToken.None);

            // Assert
            repositoryMock.Verify(repo => repo.SaveExchangeRates(It.IsAny<string>(), It.IsAny<ExchangeRateDataset>()), Times.Once); Assert.Equal("USD", result.SourceCurrency.Code);
            Assert.Equal(new DateOnly(2024, 6, 4), result.Date);
            Assert.Equal(2, result.ExchangeRates.Count());
        }

        [Fact]
        public async Task GetDailyExchangeRates_ThrowsException_WhenNoFreshDataFound()
        {
            // Arrange
            var repositoryMock = new Mock<IExchangeRateRepository>();
            var exchangeRateProviderFactoryMock = new Mock<IExchangeRateProviderFactory>();
            var loggerMock = new Mock<ILogger<HostedExchangeRateService>>();
            var recurringJobManagerMock = new Mock<IRecurringJobManager>();

            var service = new HostedExchangeRateService(
                exchangeRateProviderFactoryMock.Object,
                repositoryMock.Object,
                loggerMock.Object,
                recurringJobManagerMock.Object
            );

            var request = new FetchDailyExchangeRateRequestInternal(
                new Currency("USD"),
                new DateOnly(2024, 6, 4),
                Language.EN,
                new List<Currency> { new Currency("GBP") }
            );

            repositoryMock.Setup(repo => repo.GetExchangeRates(It.IsAny<string>())).ReturnsAsync((ExchangeRateDataset)null); // No cached data

            var providerMock = new Mock<IExchangeRateProvider>();
            exchangeRateProviderFactoryMock.Setup(factory => factory.GetProvider(request.BaseCurrency)).Returns(providerMock.Object);
            providerMock.Setup(provider => provider.GetDailyExchangeRates(request.Date, request.Language, CancellationToken.None)).ReturnsAsync(new List<ExchangeRateData>());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ExchangeRateUpdaterException>(() => service.GetDailyExchangeRatesAsync(request, CancellationToken.None));
            Assert.Equal("No exchange rate data found for specified date", exception.Message);
            repositoryMock.Verify(repo => repo.SaveExchangeRates(It.IsAny<string>(), It.IsAny<ExchangeRateDataset>()), Times.Never);
        }

        [Fact]
        public async Task UpdateDailyExchangeRates_DoesNotUpdate_WhenDataAlreadyCachedFromWorkerChannel()
        {
            // Arrange
            var repositoryMock = new Mock<IExchangeRateRepository>();
            var exchangeRateProviderFactoryMock = new Mock<IExchangeRateProviderFactory>();
            var loggerMock = new Mock<ILogger<HostedExchangeRateService>>();
            var recurringJobManagerMock = new Mock<IRecurringJobManager>();

            var service = new HostedExchangeRateService(
                exchangeRateProviderFactoryMock.Object,
                repositoryMock.Object,
                loggerMock.Object,
                recurringJobManagerMock.Object
            );

            var currency = new Currency("USD");
            var date = new DateOnly(2024, 6, 4);
            var cancellationToken = new CancellationToken();

            var cachedData = new ExchangeRateDataset(date, [new ExchangeRateData("Euro", "EUR", "Europe", 1.02m)], ExchangeRateDataset.Channel.Worker); // Cached data from worker channel
            repositoryMock.Setup(repo => repo.GetExchangeRates(It.IsAny<string>())).ReturnsAsync(cachedData);

            var providerMock = new Mock<IExchangeRateProvider>();
            exchangeRateProviderFactoryMock.Setup(factory => factory.GetProvider(currency)).Returns(providerMock.Object);

            // Act
            await service.UpdateDailyExchangeRatesAsync(currency, date, cancellationToken);

            // Assert
            repositoryMock.Verify(repo => repo.SaveExchangeRates(It.IsAny<string>(), It.IsAny<ExchangeRateDataset>()), Times.Never); // Repository Save method not called
        }

        [Fact]
        public async Task UpdateDailyExchangeRates_UpdatesCachedData_WhenCacheDataIsNotFromWorker()
        {
            // Arrange
            var repositoryMock = new Mock<IExchangeRateRepository>();
            var exchangeRateProviderFactoryMock = new Mock<IExchangeRateProviderFactory>();
            var loggerMock = new Mock<ILogger<HostedExchangeRateService>>();
            var recurringJobManagerMock = new Mock<IRecurringJobManager>();

            var service = new HostedExchangeRateService(
                exchangeRateProviderFactoryMock.Object,
                repositoryMock.Object,
                loggerMock.Object,
                recurringJobManagerMock.Object
            );

            var currency = new Currency("USD");
            var date = new DateOnly(2024, 6, 4);
            var cancellationToken = new CancellationToken();

            var freshData = new List<ExchangeRateData> { new("Euro", "EUR", "Europe", 0.80m) };

            var cachedData = new ExchangeRateDataset(date, [new ExchangeRateData("Euro", "EUR", "Europe", 0.85m)], ExchangeRateDataset.Channel.Direct); // Cached data is from Direct channel
            repositoryMock.Setup(repo => repo.GetExchangeRates(It.IsAny<string>())).ReturnsAsync(cachedData);

            exchangeRateProviderFactoryMock.Setup(factory => factory.GetProvider(currency)).Returns(Mock.Of<IExchangeRateProvider>());
            Mock.Get(exchangeRateProviderFactoryMock.Object.GetProvider(currency)).Setup(provider => provider.GetDailyExchangeRates(date, Language.EN, cancellationToken)).ReturnsAsync(freshData);
            Mock.Get(exchangeRateProviderFactoryMock.Object.GetProvider(currency)).Setup(provider => provider.GetSupportedLanguages()).Returns([Language.EN]);

            // Act
            await service.UpdateDailyExchangeRatesAsync(currency, date, cancellationToken);

            // Assert
            repositoryMock.Verify(repo => repo.SaveExchangeRates(It.IsAny<string>(), It.IsAny<ExchangeRateDataset>()), Times.Once); // Repository Save method called once
        }

    }
}
