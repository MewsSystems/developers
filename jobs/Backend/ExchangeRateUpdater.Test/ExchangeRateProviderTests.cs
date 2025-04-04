using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateProviderTests
{
    private readonly Mock<IExchangeRateService> _mockExchangeRateService;
    private readonly ExchangeRateProvider _exchangeRateProvider;
    private readonly IEnumerable<Currency> _currencies;

    public ExchangeRateProviderTests()
    {
        _mockExchangeRateService = new Mock<IExchangeRateService>();
        _exchangeRateProvider = new ExchangeRateProvider(_mockExchangeRateService.Object, new Mock<ILogger<ExchangeRateProvider>>().Object);

        _currencies = new List<Currency>
            {
                new Currency("CZK"),
                new Currency("AUD"),
                new Currency("BRL"),
                new Currency("BGN")
            };
    }

    [Fact]
    public async Task GetExchangeRates_ReturnsCachedRates()
    {
        _mockExchangeRateService.Setup(x => x.GetExchangeRatesData()).ReturnsAsync(@"
            03 Apr 2025 #66
            Country|Currency|Amount|Code|Rate
            Australia|dollar|1|AUD|14.326");

        var result = await _exchangeRateProvider.GetExchangeRates(_currencies);

        result.Should().HaveCount(1);
        result.Single().Value.Should().Be(14.326m);

        _mockExchangeRateService.Verify(x => x.GetExchangeRatesData(), Times.Once);

        // Change source data, it should not affect the cached result

        _mockExchangeRateService.Setup(x => x.GetExchangeRatesData()).ReturnsAsync(@"
            03 Apr 2025 #66
            Country|Currency|Amount|Code|Rate
            Australia|dollar|1|AUD|999.999
            Hungary|forint|100|HUF|999.999");

        var result2 = await _exchangeRateProvider.GetExchangeRates(_currencies);

        // Still should return the cached result
        result2.Should().HaveCount(1);
        result.Single().Value.Should().Be(14.326m);

        // service was not called again
        _mockExchangeRateService.Verify(x => x.GetExchangeRatesData(), Times.Once);
    }
}