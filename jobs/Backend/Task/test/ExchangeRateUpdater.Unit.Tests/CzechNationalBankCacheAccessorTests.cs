using ExchangeRateUpdater.Infrastructure.Cache;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Unit.Tests;

public class CzechNationalBankCacheAccessorTests
{
    private readonly ICzechNationalBankCacheAccessor _czechNationalBankCacheAccessor;

    private readonly Mock<IDistributedCache> _cache;

    public CzechNationalBankCacheAccessorTests()
    {
        var logger = new Mock<ILogger<CzechNationalBankCacheAccessor>>();
        _cache = new Mock<IDistributedCache>();
        _czechNationalBankCacheAccessor = new CzechNationalBankCacheAccessor(logger.Object, _cache.Object);
    }

    [Fact]
    public async Task GivenCacheIsDown_WhenGetAsyncIsExecuted_ThenNoExceptionIsThrown()
    {
        _cache
            .Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var exception = await Record.ExceptionAsync(_czechNationalBankCacheAccessor.GetAsync);

        exception.Should().BeNull();
    }

    [Fact]
    public async Task GivenCacheIsDown_WhenSetAsyncIsExecuted_ThenNoExceptionIsThrown()
    {
        _cache
            .Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var exception = await Record.ExceptionAsync(() => _czechNationalBankCacheAccessor.SetAsync([]));

        exception.Should().BeNull();
    }
}