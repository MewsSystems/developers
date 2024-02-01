using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain.Types;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.ApiClients;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.ExtensionMethods;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.Models;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Serilog;

namespace ExchangeRateUpdater.Tests
{
    public class CzechNationalBankExchangeRateProviderServiceTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<IApiClient> _exchangeRateProviderApiClientMock;
        private readonly TestCache _testCache;

        private readonly IExchangeRateProviderService _exchangeRateProviderServiceWithTestCache;

        public CzechNationalBankExchangeRateProviderServiceTests()
        {
            _loggerMock = new Mock<ILogger>();
            _exchangeRateProviderApiClientMock = new Mock<IApiClient>();
            _testCache = new TestCache();

            _exchangeRateProviderServiceWithTestCache = new CzechNationalBankExchangeRateProviderService(_loggerMock.Object, _testCache, _exchangeRateProviderApiClientMock.Object);
        }

        private void EnsureCacheIsClear()
        {
            if (_testCache.TryGetValue(DateTime.UtcNow.ToCacheKeyReferenceString(), out _))
                _testCache.Remove(DateTime.UtcNow.ToCacheKeyReferenceString());
        }

        [Fact]
        public async Task GetExchangeRates_WhenCached_ReturnsCachedExchangeRates()
        {
            // Arrange
            var cacheKey = DateTime.UtcNow.ToCacheKeyReferenceString();

            var expectedExchangeRates = NonNullResponse<Dictionary<string, ExchangeRate>>.Success(new Dictionary<string, ExchangeRate>()
            {
                { "rate1", new ExchangeRate(new Currency("TST"), new Currency("USD"), 1.22m) },
                { "rate2", new ExchangeRate(new Currency("TST"), new Currency("CZK"), 0.82m) }
            });
            
            var exchangeRates = expectedExchangeRates.Content;
            _testCache.Set(cacheKey, exchangeRates, TimeSpan.FromMinutes(1));

            // Act
            var result = await _exchangeRateProviderServiceWithTestCache.GetExchangeRates();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedExchangeRates.Content.Count, result.Content.Count);
            Assert.Equal(expectedExchangeRates.Content["rate1"].Value, result.Content["rate1"].Value);
            Assert.Equal(expectedExchangeRates.Content["rate2"].Value, result.Content["rate2"].Value);
        }

        [Fact]
        public async Task GetExchangeRates_WhenNotCached_SetsCacheAndGetExchangeRates()
        {
            // Arrange
            var centralBankRatesFromApiResult = NonNullResponse<IEnumerable<RateDto>>.Success(new List<RateDto>() { new() {CurrencyCode = "USD", Rate = 1.22m }});
            var otherCurrenciesRatesFromApiResult = NonNullResponse<IEnumerable<RateDto>>.Success(new List<RateDto>() { new() { CurrencyCode = "GBP", Rate = 0.92m } });
            var expectedExchangeRates =  NonNullResponse<Dictionary<string, ExchangeRate>>.Success(new Dictionary<string, ExchangeRate>()
            {
                { "USD", new ExchangeRate(new Currency("USD"), new Currency("CZK"), 1.22m) },
                { "GBP", new ExchangeRate(new Currency("GBP"), new Currency("CZK"), 0.92m) }
            });

            _exchangeRateProviderApiClientMock.Setup(mock => mock.GetCentralBankRates(It.IsAny<string>())).ReturnsAsync(centralBankRatesFromApiResult);
            _exchangeRateProviderApiClientMock.Setup(mock => mock.GetOtherCurrenciesRates(It.IsAny<string>())).ReturnsAsync(otherCurrenciesRatesFromApiResult);

            EnsureCacheIsClear();

            // Act
            var result = await _exchangeRateProviderServiceWithTestCache.GetExchangeRates();

            // Assert
            _exchangeRateProviderApiClientMock.Verify(mock => mock.GetCentralBankRates(It.IsAny<string>()), Times.Once);
            _exchangeRateProviderApiClientMock.Verify(mock => mock.GetOtherCurrenciesRates(It.IsAny<string>()), Times.Once);

            Assert.True(result.IsSuccess);
            Assert.Equal(expectedExchangeRates.Content.Count, result.Content.Count);
            Assert.Equal(expectedExchangeRates.Content["USD"].Value, result.Content["USD"].Value);
            Assert.Equal(expectedExchangeRates.Content["GBP"].Value, result.Content["GBP"].Value);

        }

        [Fact]
        public async Task GetExchangeRates_When_OneApiCallNotSuccessful_ReturnsSuccess_And_LogsAnError()
        {
            // Arrange
            var centralBankRatesFromApiResult =  NonNullResponse<IEnumerable<RateDto>>.Fail(new List<RateDto>(), "test message");
            var otherCurrenciesRatesFromApiResult = NonNullResponse<IEnumerable<RateDto>>.Success(new List<RateDto> { new() { CurrencyCode = "GBP", Rate = 0.92m } });

            _exchangeRateProviderApiClientMock.Setup(mock => mock.GetCentralBankRates(It.IsAny<string>())).ReturnsAsync(centralBankRatesFromApiResult);
            _exchangeRateProviderApiClientMock.Setup(mock => mock.GetOtherCurrenciesRates(It.IsAny<string>())).ReturnsAsync(otherCurrenciesRatesFromApiResult);

            _loggerMock.Setup(mock => mock.Error(It.IsAny<string>(), It.IsAny<object[]>()));

            EnsureCacheIsClear();
            // Act
            var result = await _exchangeRateProviderServiceWithTestCache.GetExchangeRates();

            // Assert
            _exchangeRateProviderApiClientMock.Verify(mock => mock.GetCentralBankRates(It.IsAny<string>()), Times.Once);
            _exchangeRateProviderApiClientMock.Verify(mock => mock.GetOtherCurrenciesRates(It.IsAny<string>()), Times.Once);

            _loggerMock.Verify(mock => mock.Error(It.IsAny<string>(), It.IsAny<NonNullResponse<IEnumerable<RateDto>>>()), Times.Once);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetExchangeRates_When_AllApiCallsNotSuccessful_ReturnsFail_And_LogsAnError()
        {
            // Arrange
            var centralBankRatesFromApiResult = NonNullResponse<IEnumerable<RateDto>>.Fail(new List<RateDto>(), "test message");
            var otherCurrenciesRatesFromApiResult = NonNullResponse<IEnumerable<RateDto>>.Fail(new List<RateDto>(), "test message");

            _exchangeRateProviderApiClientMock.Setup(mock => mock.GetCentralBankRates(It.IsAny<string>())).ReturnsAsync(centralBankRatesFromApiResult);
            _exchangeRateProviderApiClientMock.Setup(mock => mock.GetOtherCurrenciesRates(It.IsAny<string>())).ReturnsAsync(otherCurrenciesRatesFromApiResult);

            _loggerMock.Setup(mock => mock.Error(It.IsAny<string>(), It.IsAny<object[]>()));

            EnsureCacheIsClear();
            // Act
            var result = await _exchangeRateProviderServiceWithTestCache.GetExchangeRates();

            // Assert
            _exchangeRateProviderApiClientMock.Verify(mock => mock.GetCentralBankRates(It.IsAny<string>()), Times.Once);
            _exchangeRateProviderApiClientMock.Verify(mock => mock.GetOtherCurrenciesRates(It.IsAny<string>()), Times.Once);

            _loggerMock.Verify(mock => mock.Error(It.IsAny<string>(), It.IsAny<NonNullResponse<IEnumerable<RateDto>>>(),It.IsAny<NonNullResponse<IEnumerable<RateDto>>>()), Times.Once);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task GetExchangeRates_WhenThrows_ReturnsFail_AndLogs()
        {
            // Arrange
            _exchangeRateProviderApiClientMock.Setup(mock => mock.GetCentralBankRates(It.IsAny<string>())).Throws(new Exception("test"));

            _loggerMock.Setup(mock => mock.Error(It.IsAny<string>(), It.IsAny<object[]>()));

            EnsureCacheIsClear();
            // Act
            var result = await _exchangeRateProviderServiceWithTestCache.GetExchangeRates();

            // Assert
            _exchangeRateProviderApiClientMock.Verify(mock => mock.GetCentralBankRates(It.IsAny<string>()), Times.Once);

            _loggerMock.Verify(mock => mock.Error(It.IsAny<Exception>(),It.IsAny<string>()), Times.Once);

            Assert.False(result.IsSuccess);
        }

    }
}
