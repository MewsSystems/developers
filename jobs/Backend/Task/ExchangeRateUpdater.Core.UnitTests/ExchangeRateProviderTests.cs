using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Services;
using FluentAssertions;
using Moq;

namespace ExchangeRateUpdater.Core.UnitTests;

public class ExchangeRateProviderTests
{
    private IExchangeRateProvider _sut = null!;
    private Mock<IExchangeRateHttpClient> _exchangeRateHttpClientMock = null!;
    
    [SetUp]
    public void Setup()
    {
        _exchangeRateHttpClientMock = new Mock<IExchangeRateHttpClient>();
        _sut = new ExchangeRateProvider(_exchangeRateHttpClientMock.Object);
    }

    [Test]
    public void Given_Currencies_When_GetExchangeRates_Then_Expected_ExchangeRates()
    {
        // given
        var currencies = new[]
        {
            new Currency("EUR"),
            new Currency("GBP"),
            new Currency("USD")
        };
        
        
        
        // when
        var result = _sut.GetExchangeRates(currencies);

        // then
        result.Should()
            .NotBeNullOrEmpty();
    }
}