using Exchange.Application.Abstractions.ApiClients;
using Exchange.Application.Abstractions.Caching;
using Exchange.Infrastructure.ApiClients;
using Exchange.Infrastructure.DateTimeProviders;
using FluentAssertions;
using Moq;

namespace Exchange.Infrastructure.UnitTests.ApiClients;

public class CnbApiClientCacheDecoratorTests
{
    private readonly Mock<ICnbApiClient> _mockCnbApiClient = new();
    private readonly Mock<ICacheService> _mockCacheService = new();
    private readonly Mock<IDateTimeProvider> _mockDateTimeProvider = new();
    private readonly Mock<ICnbApiClientDataUpdateCalculator> _mockUpdateCalculator = new();

    private readonly List<CnbExchangeRate> _sampleExchangeRates =
    [
        new("2023-01-01", 1, "USA", "Dollar", 1, "USD", 22.5)
    ];

    [Fact]
    public async Task GetExchangeRatesAsync_WhenExchangeRatesCached_ReturnsFromCache()
    {
        // Arrange
        var currentDate = new DateTime(2023, 1, 1, 13, 0, 0);
        _mockDateTimeProvider.Setup(x => x.Now).Returns(currentDate);
        _mockCacheService
            .Setup(x => x.GetAsync<IEnumerable<CnbExchangeRate>>(It.IsAny<string>()))
            .ReturnsAsync(_sampleExchangeRates);

        var decorator = new CnbApiClientCacheDecorator(
            _mockCnbApiClient.Object,
            _mockCacheService.Object,
            _mockUpdateCalculator.Object,
            _mockDateTimeProvider.Object
        );

        // Act
        var result = await decorator.GetExchangeRatesAsync();

        // Assert
        result.Should().BeEquivalentTo(_sampleExchangeRates);
        _mockCnbApiClient.Verify(x => x.GetExchangeRatesAsync(It.IsAny<CancellationToken>()), Times.Never);
        _mockCacheService.Verify(
            x => x.SetAsync(It.IsAny<string>(), It.IsAny<IEnumerable<CnbExchangeRate>>(), It.IsAny<TimeSpan>()),
            Times.Never);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WhenExchangeRatesNotCached_ReturnsFromApi()
    {
        // Arrange
        IEnumerable<CnbExchangeRate>? cachedData = null;
        _mockCacheService
            .Setup(x => x.GetAsync<IEnumerable<CnbExchangeRate>>(It.IsAny<string>()))
            .ReturnsAsync(cachedData);

        _mockCnbApiClient
            .Setup(x => x.GetExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_sampleExchangeRates);


        var decorator = new CnbApiClientCacheDecorator(
            _mockCnbApiClient.Object,
            _mockCacheService.Object,
            _mockUpdateCalculator.Object,
            _mockDateTimeProvider.Object
        );

        // Act
        var result = await decorator.GetExchangeRatesAsync();

        // Assert
        result.Should().BeEquivalentTo(_sampleExchangeRates);
        _mockCnbApiClient.Verify(x => x.GetExchangeRatesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockCacheService.Verify(x => x.SetAsync(
                It.IsAny<string>(), It.IsAny<IEnumerable<CnbExchangeRate>>(), It.IsAny<TimeSpan>()
            ), Times.Once
        );
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WhenPastUpdateTimeAndApiResponseNotUpdated_UseShortAbsoluteExpiration()
    {
        // Arrange
        IEnumerable<CnbExchangeRate>? cachedData = null;
        _mockCacheService
            .Setup(x => x.GetAsync<IEnumerable<CnbExchangeRate>>(It.IsAny<string>()))
            .ReturnsAsync(cachedData);

        _mockCnbApiClient
            .Setup(x => x.GetExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_sampleExchangeRates);

        var expectedUpdateDate = new DateTime(2023, 1, 1, 14, 30, 0);
        var currentDateTime = new DateTime(2023, 1, 1, 15, 0, 0);
        var shortAbsoluteExpiration = TimeSpan.FromMinutes(10);

        _mockUpdateCalculator
            .Setup(x => x.GetNextExpectedUpdateDate(It.IsAny<DateOnly>()))
            .Returns(expectedUpdateDate);

        _mockDateTimeProvider.Setup(x => x.Now).Returns(currentDateTime);

        var decorator = new CnbApiClientCacheDecorator(
            _mockCnbApiClient.Object,
            _mockCacheService.Object,
            _mockUpdateCalculator.Object,
            _mockDateTimeProvider.Object
        );

        // Act
        var result = await decorator.GetExchangeRatesAsync();

        // Assert
        result.Should().BeEquivalentTo(_sampleExchangeRates);
        _mockCnbApiClient.Verify(x => x.GetExchangeRatesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockCacheService.Verify(x => x.SetAsync(
                It.IsAny<string>(), It.IsAny<IEnumerable<CnbExchangeRate>>(), shortAbsoluteExpiration),
            Times.Once
        );
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WhenPastUpdateTimeAndApiResponseUpdated_UseCalculatedExpiration()
    {
        // Arrange
        var updatedExchangeRates = new List<CnbExchangeRate>
        {
            new("2023-01-02", 1, "USA", "Dollar", 1, "USD", 23.0)
        };

        IEnumerable<CnbExchangeRate>? cachedData = null;
        _mockCacheService
            .Setup(x => x.GetAsync<IEnumerable<CnbExchangeRate>>(It.IsAny<string>()))
            .ReturnsAsync(cachedData);

        _mockCnbApiClient
            .Setup(x => x.GetExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedExchangeRates);

        var currentDateTime = new DateTime(2023, 1, 2, 15, 0, 0);
        var nextUpdateDateTime = new DateTime(2023, 1, 3, 14, 30, 0);
        var expectedCacheExpiration = nextUpdateDateTime - currentDateTime;

        _mockDateTimeProvider.Setup(x => x.Now).Returns(currentDateTime);
        _mockUpdateCalculator
            .Setup(x => x.GetNextExpectedUpdateDate(new DateOnly(2023, 1, 2)))
            .Returns(nextUpdateDateTime);

        var decorator = new CnbApiClientCacheDecorator(
            _mockCnbApiClient.Object,
            _mockCacheService.Object,
            _mockUpdateCalculator.Object,
            _mockDateTimeProvider.Object
        );

        // Act
        var result = await decorator.GetExchangeRatesAsync();

        // Assert
        result.Should().BeEquivalentTo(updatedExchangeRates);
        _mockCnbApiClient.Verify(x => x.GetExchangeRatesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockCacheService.Verify(x => x.SetAsync(
                It.IsAny<string>(), It.IsAny<IEnumerable<CnbExchangeRate>>(), expectedCacheExpiration),
            Times.Once
        );
        _mockUpdateCalculator.Verify(x => x.GetNextExpectedUpdateDate(new DateOnly(2023, 1, 2)), Times.Once);
    }
}