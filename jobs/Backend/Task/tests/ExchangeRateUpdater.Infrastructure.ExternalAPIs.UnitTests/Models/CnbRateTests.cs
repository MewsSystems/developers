using ExchangeRateUpdater.Domain.Constants;
using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Models;
using FluentAssertions;

namespace ExchangeRateUpdater.Infrastructure.ExternalAPIs.UnitTests.Models;

public class CnbRateTests
{
    [Theory]
    [InlineData(0.5, 1, 0.5)]
    [InlineData(23, 10, 2.3)]
    [InlineData(1253, 100, 12.53)]
    [InlineData(0.4, 100, 0.004)]
    public void ConvertToExchangeRate_Should_Scale_Rate(decimal cnbRate, int cnbAmount, decimal expectedRate)
    {
        var rate = new CnbRate(cnbAmount, "Test Country", "Dollar", "USD", 88, cnbRate, DateTime.UtcNow);
        var exchangeRate = rate.ToExchangeRate(CurrencyCodes.CzechKoruna);

        exchangeRate.Value.Should().Be(expectedRate);
        exchangeRate.SourceCurrency.Code.Should().Be(rate.CurrencyCode);
        exchangeRate.TargetCurrency.Code.Should().Be(CurrencyCodes.CzechKoruna);
    }
}