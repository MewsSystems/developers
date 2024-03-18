using System.Net;
using System.Net.Http.Json;
using CNB.ApiClient;
using CNB.ApiClient.Exceptions;
using CNB.ApiClient.Models;
using RichardSzalay.MockHttp;

namespace CNBApiClientTests;

public class CNBApiClientTests
{
    private readonly CNBApiClient _sut;
    private readonly MockHttpMessageHandler _handlerMock;
    private readonly DateOnly _date;

    public CNBApiClientTests()
    {
        _handlerMock = new();
        _date = DateOnly.FromDateTime(DateTime.Now);
        var httpClient = new HttpClient(_handlerMock)
        {
            BaseAddress = new Uri("http://test.cbn.api")
        };
        _sut = new CNBApiClient(httpClient);
    }

    [Fact]
    public async Task GetDailyExrates_ReturnsExrates_WhenSuccessfulResponse()
    {
        //Arrange
        var exratesDailyResponse = new ExratesDailyResponse
        {
            Rates =
            [
                new ExrateApiModel
                {
                    Amount = 1,
                    Country = "EMU",
                    Currency = "euro",
                    CurrencyCode = "EUR",
                    Rate = 25.15,
                    Order = 54,
                    ValidFor = DateTime.Parse("2024-03-15")
                }
            ]
        };

        var formattedDate = _date.ToString("yyyy-MM-dd");

        _handlerMock
            .When($"http://test.cbn.api/cnbapi/exrates/daily?date={formattedDate}&lang=EN")
            .Respond(HttpStatusCode.OK, JsonContent.Create(exratesDailyResponse));

        //Act
        var result = await _sut.GetDailyExrates(_date, CancellationToken.None);

        //Assert
        Assert.Equivalent(exratesDailyResponse.Rates.First(), result.Rates.First());
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.BadGateway)]
    [InlineData(HttpStatusCode.GatewayTimeout)]
    public async Task GetDailyExrates_ThrowsCNBApiException_WhenUnsuccessfulResponse(
        HttpStatusCode httpStatusCode
    )
    {
        //Arrange
        var formattedDate = _date.ToString("yyyy-MM-dd");

        _handlerMock
            .When($"http://test.cbn.api/cnbapi/exrates/daily?date={formattedDate}&lang=EN")
            .Respond(httpStatusCode);

        //Act and Assert
        var exception = await Assert.ThrowsAsync<CNBApiException>(
            async () => await _sut.GetDailyExrates(_date, CancellationToken.None)
        );
        Assert.Equal("CNB Api failed to handle exrates/daily request.", exception.Message);
    }
}
