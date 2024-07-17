using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Moq.Language.Flow;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace ExchangeRateUpdateTests;

public class CnbApiClientTests
{
    [Fact]
    public async Task CanGetRatesFromApi()
    {
        // Arrange

        var fakeExchangeRates = new DailyExRatesResponse()
        {
            Rates = new()
            {
                new(100, "Atlantis", "Clams", "ASC", 136, 1.2m, DateTime.Today),
                new(100, "Limuria", "Shells", "LSS", 136, .98m, DateTime.Today),
                new(100, "Faerûn", "Dungeons", "FSD", 136, .5m, DateTime.Today),
            }
        };

        Mock<HttpMessageHandler> mockHttpMessageHandler = new();
        SetupSendAsync(mockHttpMessageHandler, HttpMethod.Get)
            .ReturnsAsync(() =>
                new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(fakeExchangeRates))
                });
            //.Verifiable(Times.Exactly(1));

        var httpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost/")
        };

        CnbApiClient apiClient = new(httpClient);

        // Act
        var rates = await apiClient.GetExchangeRatesFromCnbApi(CancellationToken.None);

        // Assert
        Assert.NotEmpty(rates);
        Assert.Equal(3, rates.Count);
        mockHttpMessageHandler.Verify();
    }

    private static ISetup<HttpMessageHandler, Task<HttpResponseMessage>> SetupSendAsync(Mock<HttpMessageHandler> handler, HttpMethod requestMethod)
    {
        return handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
            ItExpr.Is<HttpRequestMessage>(r =>
                r.Method == requestMethod &&
                r.RequestUri != null
            ),
            ItExpr.IsAny<CancellationToken>()
        );
    }
}