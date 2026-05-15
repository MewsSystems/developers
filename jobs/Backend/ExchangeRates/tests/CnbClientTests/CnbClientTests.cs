using CNB.Client;
using CNB.Client.Exceptions;
using CNB.Client.Models;
using MockHttp;
using Newtonsoft.Json;
using System.Net.Mime;

namespace CnbClientTests;

public class CnbClientTests
{
    private readonly CnbClient _sut;
    private readonly MockHttpHandler _mockHttpHandler = new();
    private const string BaseUrl = "https://test.com";

    private readonly DateOnly _today = new DateOnly(2024, 03, 01);

    public CnbClientTests()
    {
        _sut = CreateSut();
    }

    private CnbClient CreateSut()
    {
        HttpClient client = new(_mockHttpHandler);
        client.BaseAddress = new(BaseUrl);
        client.DefaultRequestHeaders.Add("Accept", MediaTypeNames.Application.Json);
        return new(client);
    }

    [Fact]
    public async Task GetRatesDaily_ReturnsRates_WhenResponseIsSuccessful()
    {
        // Arrange
        var rates = new List<CnbRate> { new CnbRate { Amount = 1, Country = "USA", Currency = "USD", CurrencyCode = "USD", Order = 10, ValidFor = "2024-03-01", Rate = 10 } };
        var response = new StringContent(JsonConvert.SerializeObject(new CbnResponse { Rates = rates }));

        _mockHttpHandler
            .When(matching => matching
                .Method(HttpMethod.Get)
                .RequestUri($"{BaseUrl}/cnbapi/exrates/daily*")
            )
            .Respond(with => with
                .StatusCode(200)
                .Body(response)
            )
            .Verifiable();

        // Act
        var result = await _sut.GetRatesDaily(_today);

        // Assert
        Assert.Collection(rates, x => Assert.Equivalent(x.ToDomain(), result.First()));

    }

    [Fact]
    public async Task GetRatesDaily_ThrowsBankApiException_WhenResponseIsUnsuccessful()
    {
        // Arrange
        _mockHttpHandler
            .When(matching => matching
                .Method(HttpMethod.Get)
                .RequestUri($"{BaseUrl}/cnbapi/exrates/daily*")
            )
            .Respond(with => with
                .StatusCode(500)
             )
            .Verifiable();

        // Act & Assert
        await Assert.ThrowsAsync<BankApiException>(() => _sut.GetRatesDaily(_today));
    }

    [Fact]
    public async Task GetRatesDaily_ThrowsJsonDeserializationException_WhenFailsToDeserialize()
    {
        // Arrange
        var response = new StringContent("""
                                         {"wrongJson":0}

                                         """
        );
        _mockHttpHandler
            .When(matching => matching
                .Method(HttpMethod.Get)
                .RequestUri($"{BaseUrl}/cnbapi/exrates/daily*")
            )
            .Respond(with => with
                .StatusCode(200)
                .Body(response)
            )
            .Verifiable();

        // Act & Assert
        await Assert.ThrowsAsync<JsonDeserializationException>(() => _sut.GetRatesDaily(_today));
    }
}

