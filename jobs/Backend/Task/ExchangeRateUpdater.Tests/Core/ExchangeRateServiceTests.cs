using ExchangeRateUpdater.Domain.Common;
using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Extensions;
using ExchangeRateUpdater.Infrastructure.Services;
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
    private readonly ExchangeRateService _sut;
    private readonly DateOnly _testDate = new DateOnly(2025, 9, 26);

    public ExchangeRateServiceTests()
    {
        _exchangeProvider = Substitute.For<IExchangeRateProvider>();
        _exchangeCache = Substitute.For<IExchangeRateCache>();
        _exchangeRateservice = Substitute.For<ILogger<ExchangeRateService>>();
        _exchangeOptions = Substitute.For<IOptions<ExchangeRateOptions>>();

        var options = new ExchangeRateOptions
        {
            EnableCaching = true
        };

        _exchangeOptions.Value.Returns(options);

        _sut = new ExchangeRateService(_exchangeProvider, _exchangeCache, _exchangeRateservice, _exchangeOptions);
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

        _exchangeCache.GetCachedRates(Arg.Any<IEnumerable<Currency>>(), Arg.Any<DateOnly>())
                  .Returns(((IReadOnlyList<ExchangeRate>)cachedRates).AsMaybe());

        // Act
        var result = await _sut.GetExchangeRates(currencies, _testDate);

        // Assert
        result.Should().HaveCount(2);
        await _exchangeProvider.DidNotReceive().GetExchangeRatesForDate(Arg.Any<Maybe<DateOnly>>());
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

        _exchangeCache.GetCachedRates(Arg.Any<IReadOnlyList<Currency>>(), Arg.Any<DateOnly>())
                  .Returns(Maybe<IReadOnlyList<ExchangeRate>>.Nothing);

        _exchangeProvider.GetExchangeRatesForDate(Arg.Any<Maybe<DateOnly>>())
                     .Returns(((IReadOnlyCollection<ExchangeRate>)providerRates).AsMaybe());

        // Act
        var result = await _sut.GetExchangeRates(currencies, _testDate);

        // Assert
        result.Should().HaveCount(1);
        result.First().SourceCurrency.Code.Should().Be("USD");
        await _exchangeCache.Received(1).CacheRates(Arg.Any<IReadOnlyList<ExchangeRate>>());
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
