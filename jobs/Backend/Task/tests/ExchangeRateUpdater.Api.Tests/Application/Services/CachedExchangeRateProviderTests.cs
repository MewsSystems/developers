using AutoFixture;
using AutoFixture.AutoNSubstitute;
using ExchangeRateUpdater.Api.Domain;
using ExchangeRateUpdater.Api.Domain.Common;
using ExchangeRateUpdater.Api.Application.Interfaces;
using ExchangeRateUpdater.Api.Application.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ExchangeRateUpdater.Api.Tests.Application.Services;

public class CachedExchangeRateProviderTests
{
    private readonly IExchangeRateProvider _inner;
    private readonly ExchangeRateProviderDecorator _cachedExchangeRateProvider;

    public CachedExchangeRateProviderTests()
    {
        var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        _inner = fixture.Freeze<IExchangeRateProvider>();
        IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
        var logger = fixture.Freeze<ILogger<ExchangeRateProviderDecorator>>();
        _cachedExchangeRateProvider = new ExchangeRateProviderDecorator(_inner, cache, logger, TimeSpan.FromMinutes(10), TimeSpan.FromDays(1));
    }

    public class GetDailyRatesAsync : CachedExchangeRateProviderTests
    {
        [Fact]
        public async Task GetDailyRatesAsync_OnCacheMiss_CallsInnerProvider()
        {
            // Arrange
            var rates = CreateSuccessResult();
            _inner
                .GetDailyRatesAsync(Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                .Returns(rates);

            // Act
            var result = await _cachedExchangeRateProvider.GetDailyRatesAsync(null, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            await _inner.Received(1).GetDailyRatesAsync(null, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task GetDailyRatesAsync_OnCacheHit_ReturnsFromCacheWithoutCallingInner()
        {
            // Arrange
            var rates = CreateSuccessResult();
            _inner
                .GetDailyRatesAsync(Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                .Returns(rates);

            // Act — first call to populate cache
            await _cachedExchangeRateProvider.GetDailyRatesAsync(null, CancellationToken.None);
            // Second call should come from cache
            var result = await _cachedExchangeRateProvider.GetDailyRatesAsync(null, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            await _inner.Received(1).GetDailyRatesAsync(null, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task GetDailyRatesAsync_WithDifferentDates_UsesSeparateCacheKeys()
        {
            // Arrange
            var rates = CreateSuccessResult();
            _inner
                .GetDailyRatesAsync(Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                .Returns(rates);

            var date1 = new DateOnly(2025, 6, 1);
            var date2 = new DateOnly(2025, 6, 2);

            // Act
            await _cachedExchangeRateProvider.GetDailyRatesAsync(date1, CancellationToken.None);
            await _cachedExchangeRateProvider.GetDailyRatesAsync(date2, CancellationToken.None);

            // Assert — inner called twice, once for each date
            await _inner.Received(1).GetDailyRatesAsync(date1, Arg.Any<CancellationToken>());
            await _inner.Received(1).GetDailyRatesAsync(date2, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task GetDailyRatesAsync_WhenInnerReturnsFailure_DoesNotCacheResult()
        {
            // Arrange
            var error = ExchangeRateErrors.SourceUnavailable;
            var failureResult = Result<IReadOnlyCollection<ExchangeRate>>.Failure(error);
            var successResult = CreateSuccessResult();

            _inner
                .GetDailyRatesAsync(Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                .Returns(failureResult, successResult);

            // Act
            var first = await _cachedExchangeRateProvider.GetDailyRatesAsync(null, CancellationToken.None);
            var second = await _cachedExchangeRateProvider.GetDailyRatesAsync(null, CancellationToken.None);

            // Assert
            Assert.True(first.IsFailure);
            Assert.True(second.IsSuccess);
            await _inner.Received(2).GetDailyRatesAsync(null, Arg.Any<CancellationToken>());
        }

        private static Result<IReadOnlyCollection<ExchangeRate>> CreateSuccessResult()
        {
            IReadOnlyCollection<ExchangeRate> rates =
            [
                ExchangeRate.Create("USD", "CZK", 23.45m, 1, new DateOnly(2025, 6, 15)).Value,
                ExchangeRate.Create("EUR", "CZK", 25.10m, 1, new DateOnly(2025, 6, 15)).Value
            ];

            return Result<IReadOnlyCollection<ExchangeRate>>.Success(rates);
        }
    }
}
