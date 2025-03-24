using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Infrastructure.CnbApi.Models;
using ExchangeRateUpdater.Infrastructure.CnbApi;
using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Tests.Infrastructure.CnbApi;

public class CnbExchangeRateProviderTests
{
    private readonly Mock<ICnbApi> _cnbApiMock;
    private readonly CnbExchangeRateProvider _provider;

    public CnbExchangeRateProviderTests()
    {
        _cnbApiMock = new Mock<ICnbApi>();
        var logger = new Mock<ILogger<CnbExchangeRateProvider>>();
        
        _provider = new CnbExchangeRateProvider(_cnbApiMock.Object, logger.Object);
    }

    [Fact]
    public async Task GetExchangeRates_WhenCurrenciesExist_ReturnsCorrectRates()
    {
        var currencies = new[] { new Currency("USD"), new Currency("EUR") };
        
        var cnbResponse = new CnbExchangeRatesResponse
        {
            Rates =
            [
                new CnbExchangeRate {CurrencyCode = "USD", Rate = 23.5m, Amount = 1},
                new CnbExchangeRate {CurrencyCode = "EUR", Rate = 25.3m, Amount = 1}
            ]
        };

        _cnbApiMock.Setup(x => x.GetDailyRates(It.IsAny<string>()))
            .ReturnsAsync(cnbResponse);

        var exchangeRates = await _provider.GetExchangeRates(currencies);
        
        exchangeRates.Should().HaveCount(2);
        exchangeRates.Should().Contain(r => 
            r.SourceCurrency.Code == "CZK" && 
            r.TargetCurrency.Code == "USD" && 
            r.Value == 23.5m);
        exchangeRates.Should().Contain(r => 
            r.SourceCurrency.Code == "CZK" && 
            r.TargetCurrency.Code == "EUR" && 
            r.Value == 25.3m);
    }

    [Fact]
    public async Task GetExchangeRates_WhenCurrencyDoesNotExist_OmitsFromResult()
    {
        var currencies = new[] { new Currency("USD"), new Currency("XYZ") };
        var cnbResponse = new CnbExchangeRatesResponse
        {
            Rates = [new CnbExchangeRate {CurrencyCode = "USD", Rate = 23.5m, Amount = 1}]
        };

        _cnbApiMock.Setup(x => x.GetDailyRates(It.IsAny<string>()))
            .ReturnsAsync(cnbResponse);

        var exchangeRates = await _provider.GetExchangeRates(currencies);
        
        exchangeRates.Should().HaveCount(1);
        exchangeRates.Should().Contain(r => 
            r.SourceCurrency.Code == "CZK" && 
            r.TargetCurrency.Code == "USD");
    }

    [Fact]
    public async Task GetExchangeRates_WhenDateProvided_PassesDateToApi()
    {
        var currencies = new[] { new Currency("USD") };
        var date = new DateTime(2024, 3, 20);
        var expectedDateString = "2024-03-20";

        _cnbApiMock.Setup(x => x.GetDailyRates(expectedDateString))
            .ReturnsAsync(new CnbExchangeRatesResponse { Rates = []});

        await _provider.GetExchangeRates(currencies, date);

        _cnbApiMock.Verify(x => x.GetDailyRates(expectedDateString), Times.Once);
    }

    [Fact]
    public async Task GetExchangeRates_WhenAmountIsNotOne_CalculatesCorrectRate()
    {
        var currencies = new[] { new Currency("JPY") };
        var cnbResponse = new CnbExchangeRatesResponse
        {
            Rates = [new CnbExchangeRate {CurrencyCode = "JPY", Rate = 235.5m, Amount = 100}]
        };

        _cnbApiMock.Setup(x => x.GetDailyRates(It.IsAny<string>()))
            .ReturnsAsync(cnbResponse);

        var exchangeRates = await _provider.GetExchangeRates(currencies);
        
        exchangeRates.Should().HaveCount(1);
        exchangeRates.First().Should().BeEquivalentTo(new
        {
            SourceCurrency = new { Code = "CZK" },
            TargetCurrency = new { Code = "JPY" },
            Value = 2.355m
        });
    }
} 