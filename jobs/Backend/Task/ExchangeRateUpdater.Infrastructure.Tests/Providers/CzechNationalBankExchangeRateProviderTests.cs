using ExchangeRateUpdater.Application.Exceptions;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Infrastructure.Interfaces;
using ExchangeRateUpdater.Infrastructure.Providers;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Infrastructure.Tests.Providers;

public class CzechNationalBankExchangeRateProviderTests
{
    private readonly Mock<ICnbApiClient> _apiClientMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<ILogger<CzechNationalBankExchangeRateProvider>> _loggerMock;
    private readonly CzechNationalBankExchangeRateProvider _provider;

    public CzechNationalBankExchangeRateProviderTests()
    {
        _apiClientMock = new Mock<ICnbApiClient>();
        _cacheServiceMock = new Mock<ICacheService>();
        _loggerMock = new Mock<ILogger<CzechNationalBankExchangeRateProvider>>();

        _provider = new CzechNationalBankExchangeRateProvider(
            _apiClientMock.Object,
            _cacheServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_CallsApi_WhenCacheIsEmpty()
    {
        // Arrange
        var date = new DateTime(2024, 02, 10);
        var expectedKey = $"ExchangeRate:{date:yyyy-MM-dd}";

        // Ensure cache is empty
        _cacheServiceMock
            .Setup(cs => cs.GetOrCreateAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<IEnumerable<ExchangeRate>>>>()))
            .Returns((string _, Func<Task<IEnumerable<ExchangeRate>>> fetch) => fetch());

        // Mock API response
        var mockApiResponse = new CnbApiResponse
        {
            Rates = new List<CnbExchangeRate>
            {
                new CnbExchangeRate { CurrencyCode = "USD", Rate = 25.0m, Amount = 1, ValidFor = date }
            }
        };

        _apiClientMock
            .Setup(ac => ac.GetExchangeRatesAsync(date, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockApiResponse);

        // Act
        var result = await _provider.GetExchangeRatesAsync(date);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("USD", result.First().TargetCurrency.Code);
        Assert.Equal(25.0m, result.First().Value);

        // Verify that the API call was made
        _apiClientMock.Verify(ac => ac.GetExchangeRatesAsync(date, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_ThrowsNotFoundException_WhenApiReturnsEmptyData()
    {
        // Arrange
        var date = new DateTime(2024, 02, 10);
        var expectedKey = $"ExchangeRate:{date:yyyy-MM-dd}";

        // Ensure cache is empty
        _cacheServiceMock
            .Setup(cs => cs.GetOrCreateAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<IEnumerable<ExchangeRate>>>>()))
            .Returns((string _, Func<Task<IEnumerable<ExchangeRate>>> fetch) => fetch());

        // Mock API response as empty
        var mockApiResponse = new CnbApiResponse { Rates = new List<CnbExchangeRate>() };

        _apiClientMock
            .Setup(ac => ac.GetExchangeRatesAsync(date, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockApiResponse);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _provider.GetExchangeRatesAsync(date));

        // Verify API was called once
        _apiClientMock.Verify(ac => ac.GetExchangeRatesAsync(date, It.IsAny<CancellationToken>()), Times.Once);
    }
}
