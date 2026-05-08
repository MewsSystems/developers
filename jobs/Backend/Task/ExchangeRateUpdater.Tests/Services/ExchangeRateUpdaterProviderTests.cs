using System.Text.Json;
using ExchangeRateUpdater.Domain.Models;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace ExchangeRateUpdater.Tests.Services;

public class ExchangeRateUpdaterProviderTests(TestFixture fixture) : IClassFixture<TestFixture>, IAsyncLifetime
{
    private const string CzkCode = "CZK";
    private const string CacheKey = "ExchangeRates";
    private const string XmlResultString = "DummyXmlResult";
    
    public async Task InitializeAsync()
    {
        var cache = fixture.Services.GetRequiredService<IDistributedCache>();
        await cache.RemoveAsync(CacheKey);
        fixture.ApiClientMock.ClearReceivedCalls();
    }

    public Task DisposeAsync() => Task.CompletedTask;
    
    [Fact]
    public async Task WhenCacheKeyExists_ExchangeRatesShouldBeReturned()
    {
        // Arrange
        var eur = new Currency("EUR");
        var usd = new Currency("USD");
        var expectedRates = new List<ExchangeRate>
        {
            new() { SourceCurrency = new Currency(CzkCode), TargetCurrency = eur, Value = 27.178m },
            new() { SourceCurrency = new Currency(CzkCode), TargetCurrency = usd, Value = 22.615m }
        };

        var cache = fixture.Services.GetRequiredService<IDistributedCache>();
        var serialized = JsonSerializer.Serialize(expectedRates);
        await cache.SetStringAsync(CacheKey, serialized);

        // Act
        var result = await fixture.ExchangeRateProvider.GetExchangeRatesForCurrenciesAsync([eur, usd], CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedRates, opts => opts.WithStrictOrdering());
        await fixture.ApiClientMock.DidNotReceive().GetExchangeRatesXml(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task WhenCacheIsNotPresent_ExchangeRatesShouldBeFetchedAndReturned()
    {
        // Arrange
        var eur = new Currency("EUR");
        var usd = new Currency("USD");

       
        fixture.ApiClientMock.GetExchangeRatesXml(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(XmlResultString));

        var parsedRates = new List<ExchangeRate>
        {
            new() { SourceCurrency = new Currency(CzkCode), TargetCurrency = eur, Value = 27.178m },
            new() { SourceCurrency = new Currency(CzkCode), TargetCurrency = usd, Value = 22.615m }
        };
        fixture.ParserMock.ParseAsync(XmlResultString).Returns(Task.FromResult(parsedRates.AsEnumerable()));

        // Act
        var result = await fixture.ExchangeRateProvider.GetExchangeRatesForCurrenciesAsync([eur, usd], CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(parsedRates, opts => opts.WithStrictOrdering());
        await fixture.ApiClientMock.Received(1).GetExchangeRatesXml(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task WhenCurrencyIsUnsupported_ShouldIgnoreAndReturnMatchingExchangeRates()
    {
        // Arrange
        var eur = new Currency("EUR");
        var xyz = new Currency("XYZ");

       
        fixture.ApiClientMock.GetExchangeRatesXml(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(XmlResultString));

        var parsedRates = new List<ExchangeRate>
        {
            new() { SourceCurrency = new Currency(CzkCode), TargetCurrency = eur, Value = 27.178m }
        };
        
        fixture.ParserMock.ParseAsync(XmlResultString).Returns(Task.FromResult(parsedRates.AsEnumerable()));

        // Act
        var result =
            await fixture.ExchangeRateProvider.GetExchangeRatesForCurrenciesAsync([eur, xyz], CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(parsedRates, opts => opts.WithStrictOrdering());
    }

    [Fact]
    public async Task WhenXmlIsIncorrect_ShouldReturnEmptyRates()
    {
        // Arrange
        var eur = new Currency("EUR");
        var usd = new Currency("USD");
        
        fixture.ApiClientMock.GetExchangeRatesXml(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(XmlResultString));

        fixture.ParserMock.ParseAsync(XmlResultString)
            .Returns([]);
        
        var result =
            await fixture.ExchangeRateProvider.GetExchangeRatesForCurrenciesAsync([eur, usd], CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
    
}
