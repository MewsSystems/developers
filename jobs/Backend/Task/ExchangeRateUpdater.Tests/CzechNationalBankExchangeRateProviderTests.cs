using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;

namespace ExchangeRateUpdater.Tests;

public class CzechNationalBankExchangeRateProviderTests
{
    private static readonly string _baseAddress = "https://localhost:12345";
    private static readonly string _exchangeRatesEndpoint = "/rates";

    private readonly MockHttpMessageHandler _mockHttp;
    private readonly CzechNationalBankExchangeRateProvider _sut;

    public CzechNationalBankExchangeRateProviderTests()
    {
        _mockHttp = new MockHttpMessageHandler();
        _sut = new CzechNationalBankExchangeRateProvider(
            new HttpClient(_mockHttp)
            {
                BaseAddress = new(_baseAddress, UriKind.Absolute)
            },
            Options.Create(new CzechNationalBankExchangeRateProviderOptions
            {
                BaseAddress = new(_baseAddress, UriKind.Absolute),
                ExchangeRatesEndpoint = new(_exchangeRatesEndpoint, UriKind.Relative)
            }));
    }

    [Fact]
    public async Task GivenHttpRequestDoesntReturnData_WhenGettingExchangeRates_ThenNoRatesAreReturned()
    {
        // Arrange
        GetMockedRequest()
            .Respond("text/plain", string.Empty);

        // Act
        var rates = await _sut.GetExchangeRates(new Currency[] { new("EUR") });

        // Assert
        Assert.Empty(rates);
    }

    [Fact]
    public async Task GivenHttpRequestReturnsData_WhenNoMatchingCurrencies_ThenNoRatesAreReturned()
    {
        // Arrange
        GetMockedRequest()
            .Respond("text/plain", @"07 Oct 2022 #195
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|16.063
Hungary|forint|100|HUF|5.785");

        // Act
        var rates = await _sut.GetExchangeRates(new Currency[] { new("EUR") });

        // Assert
        Assert.Empty(rates);
    }

    [Fact]
    public async Task GivenHttpRequestReturnsData_WhenMatchingCurrencies_ThenRatesAreReturned()
    {
        // Arrange
        GetMockedRequest()
            .Respond("text/plain", @"07 Oct 2022 #195
Country|Currency|Amount|Code|Rate
Europe|euro|1|EUR|20.076
Hungary|forint|100|HUF|5.785");

        // Act
        var rates = await _sut.GetExchangeRates(new Currency[] { new("EUR") });

        // Assert
        var rate = Assert.Single(rates);
        Assert.Equal(20.076m, rate.Value);
        Assert.Equal(new Currency("CZK"), rate.TargetCurrency);
        Assert.Equal(new Currency("EUR"), rate.SourceCurrency);
    }

    [Fact]
    public async Task GivenHttpRequestReturnsData_WhenMatchingCurrencyWithNonOneAmount_ThenRateIsCorrect()
    {
        // Arrange
        GetMockedRequest()
            .Respond("text/plain", @"07 Oct 2022 #195
Country|Currency|Amount|Code|Rate
Europe|euro|1|EUR|20.076
Hungary|forint|100|HUF|5.785");

        // Act
        var rates = await _sut.GetExchangeRates(new Currency[] { new("HUF") });

        // Assert
        var rate = Assert.Single(rates);
        Assert.Equal(.05785m, rate.Value);
    }

    [Fact]
    public async Task GivenHttpRequestReturnsInvalidData_WhenGettingExchangeRates_ThenExceptionIsThrown()
    {
        // Arrange
        GetMockedRequest()
            .Respond("text/plain", @"Some random data that is not pipe-delimited
Some random data that is not pipe-delimited
Some random data that is not pipe-delimited
Some random data that is not pipe-delimited");

        // Act & Assert
        await Assert.ThrowsAnyAsync<Exception>(async () => await _sut.GetExchangeRates(new Currency[] { new("HUF") }));
    }

    private MockedRequest GetMockedRequest() => _mockHttp.When(
        HttpMethod.Get,
        new UriBuilder(_baseAddress)
        {
            Path = _exchangeRatesEndpoint
        }.ToString());
}
