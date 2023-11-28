using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Services;

namespace ExchangeRateUpdater.Core.UnitTests;

public class ExchangeRateProviderTests
{
    private IExchangeRateProvider _sut = null!;
    private Mock<IExchangeRateHttpClient> _exchangeRateHttpClientMock = null!;

    private readonly ExchangeRate[] _mockedExchangeRates = {
        new("CZK", "EUR", 1),
        new("CZK", "GBP", 2),
        new("CZK", "USD", 3),
    };
    
    [SetUp]
    public void Setup()
    {
        _exchangeRateHttpClientMock = new Mock<IExchangeRateHttpClient>();
        _sut = new ExchangeRateProvider(_exchangeRateHttpClientMock.Object);
    }

    [Test]
    public async Task Given_Currencies_When_GetExchangeRates_Then_Expected_ExchangeRates()
    {
        // given
        var currencies = new Currency[] { new("EUR"), new("GBP"), new("USD") };

        _exchangeRateHttpClientMock.Setup(e => e.GetExchangeRates())
            .ReturnsAsync(_mockedExchangeRates);
        
        // when
        var result = await _sut.GetExchangeRates(currencies);

        // then
        result.Should()
            .NotBeNullOrEmpty()
            .And
            .HaveCount(_mockedExchangeRates.Length);
    }
    
    [Test]
    public async Task Given_NoCurrencies_When_GetExchangeRates_Then_NoExchangeRates()
    {
        // given
        var currencies = Array.Empty<Currency>();

        _exchangeRateHttpClientMock.Setup(e => e.GetExchangeRates())
            .ReturnsAsync(_mockedExchangeRates);
        
        // when
        var result = await _sut.GetExchangeRates(currencies);

        // then
        result.Should()
            .NotBeNull()
            .And
            .BeEmpty();
    }
}