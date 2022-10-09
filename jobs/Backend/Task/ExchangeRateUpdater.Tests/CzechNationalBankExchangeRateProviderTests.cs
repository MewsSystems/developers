using System.Net;
using System.Security.Cryptography;
using RichardSzalay.MockHttp;

namespace ExchangeRateUpdater.Tests;

public class CzechNationalBankExchangeRateProviderTests : IClassFixture<Fixture>
{
    private readonly Fixture _fixture;
    private readonly CzechNationalBankExchangeRateProvider _sut;

    public CzechNationalBankExchangeRateProviderTests(Fixture fixture)
    {
        _fixture = fixture;
        _sut = _fixture.Sut;

        _fixture.MockHttp.ResetExpectations();
    }

    [Fact]
    public async Task GivenHttpRequestDoesntReturnData_WhenGettingExchangeRates_ThenNoRatesAreReturned()
    {
        // Arrange
        SetUpMockedRequest()
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
        SetUpMockedRequest()
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
        SetUpMockedRequest()
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
        SetUpMockedRequest()
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
        SetUpMockedRequest()
            .Respond("text/plain", @"Some random data that is not pipe-delimited
Some random data that is not pipe-delimited
Some random data that is not pipe-delimited
Some random data that is not pipe-delimited");

        // Act & Assert
        await Assert.ThrowsAnyAsync<Exception>(async () => await _sut.GetExchangeRates(new Currency[] { new("HUF") }));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task GivenHttpRequestFailsLessThanThreeTimesWithTransientError_WhenGettingExchangeRates_ThenOperationSucceeds(int numberOfFailures)
    {
        // Arrange
        foreach (var _ in Enumerable.Range(1, numberOfFailures))
        {
            SetUpTransientErrorMockedRequest();
        }

        SetUpMockedRequest()
            .Respond("text/plain", @"07 Oct 2022 #195
Country|Currency|Amount|Code|Rate
Europe|euro|1|EUR|20.076
Hungary|forint|100|HUF|5.785");

        // Act
        var rates = await _sut.GetExchangeRates(new Currency[] { new("HUF") });

        // Assert
        Assert.NotEmpty(rates);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(4)]
    public async Task GivenHttpRequestFailsAtLeastThreeTimesWithTransientError_WhenGettingExchangeRates_ThenExceptionIsThrown(int numberOfFailures)
    {
        // Arrange
        foreach (var _ in Enumerable.Range(1, numberOfFailures))
        {
            SetUpTransientErrorMockedRequest();
        }

        // Act & Assert
        await Assert.ThrowsAnyAsync<Exception>(async () => await _sut.GetExchangeRates(new Currency[] { new("HUF") }));
    }

    public static TheoryData<Action<MockedRequest>> NonTransientErrors => new()
    {
        { request => request.Respond(HttpStatusCode.NotFound) },
        { request => request.Respond(HttpStatusCode.Unauthorized) },
        { request => request.Respond(HttpStatusCode.Forbidden) },
        { request => request.Respond(HttpStatusCode.MethodNotAllowed) }
    };

    [Theory]
    [MemberData(nameof(NonTransientErrors))]
    public async Task GivenHttpRequestFailsWithNonTransientError_WhenGettingExchangeRates_ThenExceptionIsThrown(Action<MockedRequest> nonTransientError)
    {
        // Arrange
        var mockedRequest = SetUpMockedRequest();
        nonTransientError(mockedRequest);

        // Act & Assert
        await Assert.ThrowsAnyAsync<Exception>(async () => await _sut.GetExchangeRates(new Currency[] { new("HUF") }));
    }

    private MockedRequest SetUpMockedRequest() => _fixture.MockHttp.Expect(
        HttpMethod.Get,
        _fixture.ExchangeRatesEndpointAbsoluteUrl);

    private MockedRequest SetUpTransientErrorMockedRequest()
    {
        var transientErrors = new Action<MockedRequest>[]
        {
            // Exception
            request => request.Throw(new HttpRequestException("Transient error!")),

            // 408
            request => request.Respond(HttpStatusCode.RequestTimeout),

            // 50x
            request => request.Respond(HttpStatusCode.InternalServerError),
            request => request.Respond(HttpStatusCode.BadGateway),
            request => request.Respond(HttpStatusCode.ServiceUnavailable),
        };

        var mockedRequest = SetUpMockedRequest();
        var randomTransientError = transientErrors[RandomNumberGenerator.GetInt32(0, transientErrors.Length)];
        randomTransientError(mockedRequest);

        return mockedRequest;
    }
}
