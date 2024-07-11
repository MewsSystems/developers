using Moq;
using FluentAssertions;
using ExchangeRateUpdater.ExchangeApis;
using ExchangeRateUpdater.RateProvider;

namespace Test.Unit;

public class TestExchangeRateProvider
{
    [Fact]
    public async Task WhenCurrencyNotInSource_MissingInResult()
    {
        // Given
        var api = new Mock<IExchangeApi>();
        api.Setup(a => a.GetAllRates(It.IsAny<CancellationToken>()))
            .ReturnsAsync([new ExchangeRate("EUR", "CZK", 25)]);
        var provider = new ExchangeRateProvider(api.Object);
        var currencies = new[]
        {
            new Currency("EUR"),
            new Currency("NON_EXISTING"),
        };

        // When
        var rates = await provider.GetExchangeRates(currencies);

        // Then
        rates.Should().HaveCount(1);
        rates.Should().ContainSingle(r => r.SourceCurrency.Code == "EUR");
        rates.Should().OnlyContain(r => r.TargetCurrency.Code == "CZK");
    }

    [Fact]
    public async Task WhenEmptySource_NoResults()
    {
        // Given
        var api = new Mock<IExchangeApi>();
        api.Setup(a => a.GetAllRates(It.IsAny<CancellationToken>())).ReturnsAsync([]);
        var provider = new ExchangeRateProvider(api.Object);
        var currencies = new[]
        {
            new Currency("EUR")
        };

        // When
        var rates = await provider.GetExchangeRates(currencies);

        // Then
        rates.Should().BeEmpty();
    }

    [Fact]
    public async Task WhenEmptyCurrencies_NoResults()
    {
        // Given
        var api = new Mock<IExchangeApi>();
        api.Setup(a => a.GetAllRates(It.IsAny<CancellationToken>()))
            .ReturnsAsync([new ExchangeRate("EUR", "CZK", 25)]);
        var provider = new ExchangeRateProvider(api.Object);
        var currencies = Array.Empty<Currency>();

        // When
        var rates = await provider.GetExchangeRates(currencies);

        // Then
        rates.Should().BeEmpty();
    }

    [Fact]
    public async Task WhenEmptyCurrencies_AndEmptySource_NoResults()
    {
        // Given
        var api = new Mock<IExchangeApi>();
        api.Setup(a => a.GetAllRates(It.IsAny<CancellationToken>())).ReturnsAsync([]);
        var provider = new ExchangeRateProvider(api.Object);
        var currencies = Array.Empty<Currency>();

        // When
        var rates = await provider.GetExchangeRates(currencies);

        // Then
        rates.Should().BeEmpty();
    }
}