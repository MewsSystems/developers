using ExchangeRateUpdater.Core.Common;
using ExchangeRateUpdater.Core.Configuration;
using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Extensions;
using ExchangeRateUpdater.Core.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using FluentAssertions;
using NSubstitute;

namespace ExchangeRateUpdater.Tests.Core;

public class ExchangeRateServiceTests
{
    private readonly IExchangeRateProvider _exchangeProvider;
    private readonly IExchangeRateCache _exchangeCache;
    private readonly ILogger<ExchangeRateService> _exchangeRateservice;
    private readonly IOptions<ExchangeRateOptions> _exchangeOptions;
    private readonly IOptions<CacheSettings> _cacheSettings;
    private readonly ExchangeRateService _sut;
    private readonly DateTime _testDate = new DateTime(2025, 9, 26);

    public ExchangeRateServiceTests()
    {
        _exchangeProvider = Substitute.For<IExchangeRateProvider>();
        _exchangeCache = Substitute.For<IExchangeRateCache>();
        _exchangeRateservice = Substitute.For<ILogger<ExchangeRateService>>();
        _exchangeOptions = Substitute.For<IOptions<ExchangeRateOptions>>();
        _cacheSettings = Substitute.For<IOptions<CacheSettings>>();

        var options = new ExchangeRateOptions
        {
            EnableCaching = true
        };

        var cacheSettings = new CacheSettings
        {
            DefaultCacheExpiry = TimeSpan.FromHours(1)
        };

        _exchangeOptions.Value.Returns(options);
        _cacheSettings.Value.Returns(cacheSettings);

        _sut = new ExchangeRateService(_exchangeProvider, _exchangeCache, _exchangeRateservice, _exchangeOptions, _cacheSettings);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithCachedRates_ShouldReturnCachedRates()
    {
        // Arrange
        var currencies = new[] { new Currency("USD"), new Currency("EUR") };
        var cachedRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 25.0m, _testDate),
            new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 27.0m, _testDate)
        };

        _exchangeCache.GetCachedRates(Arg.Any<IEnumerable<Currency>>(), Arg.Any<Maybe<DateTime>>())
                  .Returns(((IReadOnlyList<ExchangeRate>)cachedRates).AsMaybe());

        // Act
        var result = await _sut.GetExchangeRates(currencies, _testDate.AsMaybe());

        // Assert
        result.Should().HaveCount(2);
        await _exchangeProvider.DidNotReceive().GetExchangeRatesForDate(Arg.Any<Maybe<DateTime>>());
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithoutCachedRates_ShouldFetchFromProvider()
    {
        // Arrange
        var currencies = new[] { new Currency("USD") };
        var providerRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 25.0m, _testDate)
        };

        _exchangeCache.GetCachedRates(Arg.Any<IReadOnlyList<Currency>>(), Arg.Any<Maybe<DateTime>>())
                  .Returns(Maybe<IReadOnlyList<ExchangeRate>>.Nothing);

        _exchangeProvider.GetExchangeRatesForDate(Arg.Any<Maybe<DateTime>>())
                     .Returns(((IReadOnlyCollection<ExchangeRate>)providerRates).AsMaybe());

        // Act
        var result = await _sut.GetExchangeRates(currencies, _testDate.AsMaybe());

        // Assert
        result.Should().HaveCount(1);
        result.First().SourceCurrency.Code.Should().Be("USD");
        await _exchangeCache.Received(1).CacheRates(Arg.Any<IReadOnlyList<ExchangeRate>>(), Arg.Any<TimeSpan>());
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithNullCurrencies_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            _sut.GetExchangeRates(null!, _testDate.AsMaybe()));
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithEmptyCurrencies_ShouldReturnEmpty()
    {
        // Arrange
        var emptyCurrencies = Array.Empty<Currency>();

        // Act
        var result = await _sut.GetExchangeRates(emptyCurrencies, _testDate.AsMaybe());

        // Assert
        result.Should().BeEmpty();
    }
}
