using ExchangeRateUpdater.Business;
using ExchangeRateUpdater.Business.Processors;
using ExchangeRateUpdater.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace ExchangeRateUpdaterTests.Business;

public class ExchangeRateProviderTests
{
    private readonly Mock<IExchangeRateProcessor> _exchangeRateProcessorMock;
    private readonly ExchangeRateProvider _provider;

    public ExchangeRateProviderTests()
    {
        _exchangeRateProcessorMock = new Mock<IExchangeRateProcessor>();
        _provider = new ExchangeRateProvider(_exchangeRateProcessorMock.Object);
    }

    [Fact]
    public async Task Given_CurrencyCodes_When_GetExchangeRates_Then_CallsProcessor()
    {
        var currencyCodes = new[] { "USD", "EUR" };
        var expectedResult = Result<IEnumerable<ExchangeRate>>.Ok(new List<ExchangeRate>());
        _exchangeRateProcessorMock
            .Setup(p => p.ProcessExchangeRates(currencyCodes))
            .ReturnsAsync(expectedResult);

        var result = await _provider.GetExchangeRates(currencyCodes);

        expectedResult.Should().Be(result);
        _exchangeRateProcessorMock.Verify(p => p.ProcessExchangeRates(currencyCodes), Times.Once);
    }
}
