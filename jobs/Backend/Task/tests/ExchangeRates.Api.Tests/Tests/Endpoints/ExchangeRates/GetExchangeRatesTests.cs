using ExchangeRates.Api.Infrastructure.Clients.Cnb.Models;
using ExchangeRates.Api.Models;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace ExchangeRates.Api.Tests.Tests.Endpoints.ExchangeRates;

[Collection(nameof(ExchangeRatesTestsContext))]
public class GetExchangeRatesTests : BaseEndpointsTests
{
    private readonly WireMockServer _cnbWireMockServer;
    private readonly IDistributedCache _distributedCache;
    private readonly HttpClient _httpClient;

    public GetExchangeRatesTests(ExchangeRatesTestsContext exchangeRatesTestsContext) : base(exchangeRatesTestsContext)
    {
        _cnbWireMockServer = exchangeRatesTestsContext.Services.GetRequiredService<WireMockServer>();
        _distributedCache = exchangeRatesTestsContext.Services.GetRequiredService<IDistributedCache>();
        _httpClient = exchangeRatesTestsContext.HttpClient;
    }

    [Fact(DisplayName = "Get Exchange Rates returns data from Cnb API")]
    public async Task GetExchangeRatesReturnsDataFromClientAsync()
    {
        var expectedCnbRate = new CnbExchangeRate
        {
            Amount = 1,
            CurrencyCode = "USD",
            Rate = 2
        };

        var expectedCnbExchangeRates = new CnbExchangeRates
        {
            Rates = [expectedCnbRate]
        };

        var cnbExchangeRateRequestBuilder = Request.Create().WithUrl("http://localhost:20000/cnbapi/exrates/daily?date=*").UsingGet();

        _cnbWireMockServer
            .Given(cnbExchangeRateRequestBuilder)
            .RespondWith(
                Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "text/plain")
                .WithBody(JsonSerializer.Serialize(expectedCnbExchangeRates)));

        var httpResponse = await _httpClient.GetAsync("/api/exchange-rates");

        httpResponse.Should().NotBeNull();
        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var actualExchangeRates = JsonSerializer.Deserialize<IEnumerable<ExchangeRate>>(await httpResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        actualExchangeRates.Should().NotBeNullOrEmpty();
        actualExchangeRates.Should().HaveCount(1);

        var actualExchageRate = actualExchangeRates!.First();

        actualExchageRate.SourceCurrency.Code.Should().Be(expectedCnbRate.CurrencyCode);
        actualExchageRate.TargetCurrency.Code.Should().Be("CZK");
        actualExchageRate.Value.Should().Be(expectedCnbRate.Rate);

        _cnbWireMockServer.FindLogEntries(cnbExchangeRateRequestBuilder).Should().HaveCount(1);
    }

    [Fact(DisplayName = "Get Exchange Rates returns data from Cache")]
    public async Task GetExchangeRatesReturnsDataFromCacheAsync()
    {
        var expectedCachedExchageRates = new List<ExchangeRate> { new(new Currency("USD"), new Currency("EUR"), 1) };

        await _distributedCache.SetStringAsync("exchange-rates", JsonSerializer.Serialize(expectedCachedExchageRates));

        var httpResponse = await _httpClient.GetAsync("/api/exchange-rates");

        httpResponse.Should().NotBeNull();
        httpResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var actualExchangeRates = JsonSerializer.Deserialize<IEnumerable<ExchangeRate>>(await httpResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        actualExchangeRates.Should().NotBeNullOrEmpty();
        actualExchangeRates.Should().BeEquivalentTo(expectedCachedExchageRates);
    }

    [Fact(DisplayName = "Get Exchange Rates returns 500 error if Cnb API returns unexpected data")]
    public async Task GetExchangeRatesReturnsErrorIdClientReturnsUnexpectedDataAsync()
    {
        var cnbExchangeRateRequestBuilder = Request.Create().WithUrl("http://localhost:20000/cnbapi/exrates/daily?date=*").UsingGet();

        _cnbWireMockServer
            .Given(cnbExchangeRateRequestBuilder)
            .RespondWith(
                Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "text/plain")
                .WithBody(JsonSerializer.Serialize(new CnbExchangeRates())));

        var httpResponse = await _httpClient.GetAsync("/api/exchange-rates");

        httpResponse.Should().NotBeNull();
        httpResponse.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        _cnbWireMockServer.FindLogEntries(cnbExchangeRateRequestBuilder).Should().HaveCount(1);
    }
}
