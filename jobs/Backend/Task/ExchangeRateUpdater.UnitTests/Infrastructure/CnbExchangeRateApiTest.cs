using ExchangeRateUpdater.Domain.Enums;
using ExchangeRateUpdater.Infrastructure.Apis;
using ExchangeRateUpdater.Infrastructure.Dtos;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace ExchangeRateUpdater.UnitTests.Infrastructure;

public class CnbExchangeRateApiTest
{
    private readonly Mock<ILogger<CnbExchangeRateApi>> _loggerMock;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock = new();
    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    const string baseUrl = "http://test.com";

    private CnbExchangeRateApi? _cnbExchangeRateApi;

    public CnbExchangeRateApiTest()
    {
        _loggerMock = new Mock<ILogger<CnbExchangeRateApi>>();
    }

    [Theory]
    [MemberData(nameof(SuccessfulCases))]
    public async Task GetExchangeRatesAsync_WhenParametersProvidedOrNot_ShouldReturnRates(DateOnly? date, Language? language, string expectedUri)
    {
        var response = new CnbExchangeRateResponse
        (
            new List<CnbExchangeRateResponseItem>
            {
                new(1, "Japan", "yen", "JPY", 80, 15.285m, ""),
                new(1, "EMU", "euro", "EUR", 80, 4.503m, ""),
                new(1, "USA", "dollar", "USD", 80, 12.750m, ""),
                new(1, "Argentina", "pesos argentinos", "ARS", 80, 16.910m, ""),
                new(1, "Country", "XYZ", "XYZ", 80, 3.342m, "")
            }
        );
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(response, jsonSerializerOptions))
        };

        _cnbExchangeRateApi = new CnbExchangeRateApi(GetHttpClientMock(httpResponseMessage), _loggerMock.Object);

        var exchangeRates = await _cnbExchangeRateApi.GetExchangeRatesAsync(date, language);

        exchangeRates.Should()
            .NotBeNullOrEmpty()
            .And.AllBeOfType(typeof(CnbExchangeRateResponseItem));
        exchangeRates.Should().BeEquivalentTo(response.Rates);
        _httpMessageHandlerMock.Protected().Verify(
           "SendAsync",
           Times.Once(),
           ItExpr.Is<HttpRequestMessage>(request => request.Method == HttpMethod.Get && request.RequestUri == new Uri(expectedUri)),
           ItExpr.IsAny<CancellationToken>()
        );
    }

    [Theory]
    [InlineData("Bad Request response",
        "Bad Request in GetExchangeRatesAsync: Bad Request response", 
        "Response status code does not indicate success: 400 (Bad Request).", 
        HttpStatusCode.BadRequest)]
    [InlineData("Resource Not Found response",
        "Not Found in GetExchangeRatesAsync: Resource Not Found response",
        "Response status code does not indicate success: 404 (Not Found).",
        HttpStatusCode.NotFound)]
    [InlineData("Internal Server Error response",
        "Internal Server Error in GetExchangeRatesAsync: Internal Server Error response",
        "Response status code does not indicate success: 500 (Internal Server Error).",
        HttpStatusCode.InternalServerError)]
    public async Task GetExchangeRatesAsync_WhenParametersProvided_ShouldReturnError(string errorMsg, string logMsg, string exMsg, HttpStatusCode statusCode)
    {
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = statusCode,
            Content = new StringContent(errorMsg)
        };
        _cnbExchangeRateApi = new CnbExchangeRateApi(GetHttpClientMock(httpResponseMessage), _loggerMock.Object);

        var act = async () => await _cnbExchangeRateApi.GetExchangeRatesAsync(new DateOnly(2024, 5, 1), Language.EN);
        
        await act.Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage(exMsg);
        _loggerMock.Verify(
            logger => logger.Log(LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains(logMsg)),
                It.IsAny<HttpRequestException>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ),
            Times.Once);
    }

    private HttpClient GetHttpClientMock(HttpResponseMessage httpResponseMessage)
    {
        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(httpResponseMessage);

        return new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri(baseUrl)
        };
    }

    public static IEnumerable<object?[]> SuccessfulCases =>
        new List<object?[]>
        {
            new object ?[] { new DateOnly(2024, 5, 1), Language.EN, $"{baseUrl}/daily?date=2024-05-01&lang=EN" },
            new object ?[] { new DateOnly(2024, 5, 1), null, $"{baseUrl}/daily?date=2024-05-01" },
            new object ?[] { null, Language.EN, $"{baseUrl}/daily?lang=EN" },
            new object ?[] { null, null, $"{baseUrl}/daily" },
        };
}
