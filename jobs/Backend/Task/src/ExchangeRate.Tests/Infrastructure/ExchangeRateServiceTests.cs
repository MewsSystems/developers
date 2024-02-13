using ExchangeRate.Infrastructure.Cnb;
using ExchangeRate.Infrastructure.Cnb.Mappers;
using ExchangeRate.Infrastructure.Cnb.Models;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace ExchangeRate.Tests.Infrastructure;

public class ExchangeRateServiceTests : TestsBase
{
    private IMemoryCache _cache = null!;
    private Mock<IExchangeRateFetcher> _mockFetcher = null!;
    private ExchangeRatesService _exchangeRatesService = null!;
    
    [SetUp]
    public void Setup()
    {
        _cache = new MemoryCache(new MemoryCacheOptions());
        _mockFetcher = new Mock<IExchangeRateFetcher>();
        _exchangeRatesService = new ExchangeRatesService(_cache, _mockFetcher.Object);
    }
    
    //Note: much more tests could be added like testing the weekends, edge cases with time when the data are refreshed on CNB
    
    [Test]
    public async Task GetCurrentExchangeRates_FetchesNewData_WhenCacheIsEmpty()
    {
        // Arrange
        var validFor = DateTime.Today;
        var numberOfRates = 3;
        var fakeRates = GenerateExchangeRates(validFor, numberOfRates);

        _mockFetcher.Setup(x => x.GetDailyExchangeRates(It.IsAny<string>())).ReturnsAsync(fakeRates);

        // Act
        var result = (await _exchangeRatesService.GetCurrentExchangeRates())?.ToList();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(numberOfRates);
        fakeRates.ToDomain().Should().BeEquivalentTo(result);
        _mockFetcher.Verify(x => x.GetDailyExchangeRates(It.IsAny<string?>()), Times.Once);
    }
    
    [Test]
    public async Task GetCurrentExchangeRates_DataFetcherIsNotCalled_WhenValidDataIsCached()
    {
        // Arrange
        var validFor = DateTime.Today;
        var numberOfRates = 3;
        var fakeRates = GenerateExchangeRates(validFor, numberOfRates);
        CachedValue? cachedValue = null;

        _mockFetcher.Setup(x => x.GetDailyExchangeRates(It.IsAny<string>())).ReturnsAsync(fakeRates);
        _cache.Set("ExchangeRates", new CachedValue(validFor, fakeRates), TimeSpan.FromHours(24));
        
        // Act
        var result = (await _exchangeRatesService.GetCurrentExchangeRates())?.ToList();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(numberOfRates);
        fakeRates.ToDomain().Should().BeEquivalentTo(result);
        _mockFetcher.Verify(x => x.GetDailyExchangeRates(It.IsAny<string?>()), Times.Never);
    }
}