using Exchange.Application.Abstractions.ApiClients;
using Exchange.Application.Services;
using Exchange.Domain.Entities;
using Exchange.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Exchange.Application.UnitTests.Services;

public class ExchangeRateProviderTests
{
    private readonly Mock<ICnbApiClient> _cnbApiClientMock = new();
    private readonly ExchangeRateProvider _sut;

    public ExchangeRateProviderTests()
    {
        _sut = new ExchangeRateProvider(_cnbApiClientMock.Object);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WhenCurrenciesExistInSource_ThenReturnsFilteredRates()
    {
        // Arrange
        IEnumerable<Currency> requestedCurrencies = [Currency.BRL, Currency.EUR];
        IEnumerable<CnbExchangeRate> cnbExchangeRates =
        [
            new("2025-01-01", 1, Currency.BRL.Country, Currency.BRL.Name, 1, Currency.BRL.Code, 3.881),
            new("2025-01-01", 1, Currency.EUR.Country, Currency.EUR.Name, 1, Currency.EUR.Code, 24.295)
        ];
        _cnbApiClientMock
            .Setup(x => x.GetExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(cnbExchangeRates);

        // Act
        var result = await _sut.GetExchangeRatesAsync(requestedCurrencies);

        // Assert
        var expectedResult = new List<ExchangeRate>()
        {
            new(Currency.BRL, Currency.CZK, 3.881m),
            new(Currency.EUR, Currency.CZK, 24.295m)
        };
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WhenNoCurrenciesMatch_ThenReturnsEmpty()
    {
        // Arrange
        IEnumerable<Currency> requestedCurrencies = [Currency.USD, Currency.GBP];
        IEnumerable<CnbExchangeRate> cnbExchangeRates =
        [
            new("2025-01-01", 1, Currency.BRL.Country, Currency.BRL.Name, 1, Currency.BRL.Code, 3.881),
            new("2025-01-01", 1, Currency.EUR.Country, Currency.EUR.Name, 1, Currency.EUR.Code, 24.295)
        ];
        _cnbApiClientMock
            .Setup(x => x.GetExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(cnbExchangeRates);

        // Act
        var result = await _sut.GetExchangeRatesAsync(requestedCurrencies);

        // Assert
        var expectedResult = Enumerable.Empty<ExchangeRate>();
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WhenAmountBiggerThanOne_ThenCalculateCalculateExchangeRateForOneCzk()
    {
        // Arrange
        IEnumerable<Currency> requestedCurrencies = [Currency.IDR];
        IEnumerable<CnbExchangeRate> cnbExchangeRates =
        [
            new("2025-01-01", 1, Currency.IDR.Country, Currency.IDR.Name, 1000, Currency.IDR.Code, 2)
        ];
        _cnbApiClientMock
            .Setup(x => x.GetExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(cnbExchangeRates);
        
        // Act
        var result = await _sut.GetExchangeRatesAsync(requestedCurrencies);
        
        // Assert
        var expectedResult = new List<ExchangeRate>()
        {
            new(Currency.IDR, Currency.CZK, 500m)
        };
        result.Should().BeEquivalentTo(expectedResult);
    }
}