using ExchangeRateUpdater.Application.Common;
using ExchangeRateUpdater.Application.ExchangeRates.GetExchangeRates;
using ExchangeRateUpdater.Core.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace ExchangeRateUpdater.Tests.Application.ExchangeRates.GetExchangeRates;

public class GetExchangeRatesQueryHandlerTests
{
    private readonly Mock<IExchangeRateProvider> _exchangeRateProviderMock;
    private readonly GetExchangeRatesQueryHandler _handler;

    public GetExchangeRatesQueryHandlerTests()
    {
        _exchangeRateProviderMock = new Mock<IExchangeRateProvider>();
        _handler = new GetExchangeRatesQueryHandler(_exchangeRateProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WhenProviderReturnsRates_ReturnsSameRates()
    {
        var currencies = new[] { new Currency("USD"), new Currency("EUR") };
        var expectedRates = new[]
        {
            new ExchangeRate(new Currency("CZK"), new Currency("USD"), 23.5m, DateTime.Today),
            new ExchangeRate(new Currency("CZK"), new Currency("EUR"), 25.3m, DateTime.Today)
        };

        _exchangeRateProviderMock.Setup(x => x.GetExchangeRates(currencies, null)).ReturnsAsync(expectedRates);

        var query = new GetExchangeRatesQuery(currencies);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo(expectedRates);
        _exchangeRateProviderMock.Verify(x => x.GetExchangeRates(currencies, null), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenDateProvided_PassesDateToProvider()
    {
        var currencies = new[] { new Currency("USD") };
        var date = new DateTime(2024, 3, 20);
        var expectedRates = new[]
        {
            new ExchangeRate(new Currency("CZK"), new Currency("USD"), 23.5m, DateTime.Today)
        };

        _exchangeRateProviderMock.Setup(x => x.GetExchangeRates(currencies, date))
            .ReturnsAsync(expectedRates);

        var query = new GetExchangeRatesQuery(currencies, date);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo(expectedRates);
        
        _exchangeRateProviderMock.Verify(x => x.GetExchangeRates(currencies, date), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenProviderReturnsEmpty_ReturnsEmpty()
    {
        var currencies = new[] { new Currency("USD") };
        var expectedRates = Array.Empty<ExchangeRate>();

        _exchangeRateProviderMock.Setup(x => x.GetExchangeRates(currencies, null))
            .ReturnsAsync(expectedRates);

        var query = new GetExchangeRatesQuery(currencies);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeEmpty();
        
        _exchangeRateProviderMock.Verify(x => x.GetExchangeRates(currencies, null), Times.Once);
    }
} 