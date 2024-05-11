using System.Net;
using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Clients;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using NSubstitute;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace ExchangeRateUpdater.Infrastructure.ExternalAPIs.UnitTests.Clients;

public class ExchangeRateClientTests : IDisposable
{
    private readonly WireMockServer _server;
    
    // Response bodies from https://api.cnb.cz/cnbapi/swagger-ui.html
    private const string OkBody = """
                                  {
                                    "rates": [
                                      {
                                        "amount": 0,
                                        "country": "string",
                                        "currency": "string",
                                        "currencyCode": "string",
                                        "order": 0,
                                        "rate": 0,
                                        "validFor": "2024-05-09"
                                      }
                                    ]
                                  }
                                  """;

    private const string BadRequestBody = """
                                          {
                                            "description": "string",
                                            "endPoint": "string",
                                            "errorCode": "BAD_REQUEST",
                                            "happenedAt": "2024-05-09T20:54:45.399Z",
                                            "messageId": "string"
                                          }
                                          """;

    private const string InternalServerErrorBody = """
                                                   {
                                                     "description": "string",
                                                     "endPoint": "string",
                                                     "errorCode": "INTERNAL_SERVER_ERROR",
                                                     "happenedAt": "2024-05-09T20:54:45.399Z",
                                                     "messageId": "string"
                                                   }
                                                   """;

    private readonly FakeLogger<ExchangeRateClient> _logger;
    private readonly ExchangeRateClient _sut;
    
    public ExchangeRateClientTests()
    {
        _server = WireMockServer.Start();
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(_server.Url);
        
        var timeProvider = Substitute.For<TimeProvider>();
        timeProvider
            .GetUtcNow()
            .Returns(new DateTimeOffset(new DateTime(2024, 5, 9)));

        _logger = new FakeLogger<ExchangeRateClient>();
        
        _sut = new ExchangeRateClient(httpClient, timeProvider, _logger);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _server.Stop();
    }

    [Fact]
    public async Task GetRates_Should_Return_Rates()
    {
        _server.Given(Request.Create().WithPath("/exrates/daily").UsingGet())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithBody(OkBody)
                );

        var rates = await _sut.GetRates(CancellationToken.None);
        
        var ratesList = rates!.ToList();

        ratesList.Should().HaveCount(1); 
        ratesList.First().CurrencyCode.Should().Be("string");
    }
    
    [Fact]
    public async Task GetRates_Should_Return_Rates_When_Date_Specified()
    {
        var date = new DateTime(2023, 3, 2);
        _server.Given(Request.Create()
                .WithPath("/exrates/daily")
                .WithParam("date", MatchBehaviour.AcceptOnMatch, date.ToString("yyyy-MM-dd"))
                .UsingGet())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithBody(OkBody)
            );

        var rates = await _sut.GetRates(date, CancellationToken.None);

        var ratesList = rates!.ToList();
        ratesList.Should().HaveCount(1);
        ratesList.First().CurrencyCode.Should().Be("string");
    }
    
    [Fact]
    public async Task GetRates_Should_Handle_BadRequest()
    {
        _server.Given(Request.Create().WithPath("/exrates/daily").UsingGet())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.BadRequest)
                    .WithBody(BadRequestBody)
            );

        var rates = await _sut.GetRates(CancellationToken.None);

        rates.Should().BeNull();
        _logger.Collector.LatestRecord.Should().NotBeNull();
        _logger.Collector.LatestRecord.Level.Should().Be(LogLevel.Error);
        _logger.Collector.LatestRecord.Message.Should().Contain("status code BadRequest");
    }
    
    [Fact]
    public async Task GetRates_Should_Handle_InternalServerError()
    {
        _server.Given(Request.Create().WithPath("/exrates/daily").UsingGet())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.InternalServerError)
                    .WithBody(InternalServerErrorBody)
            );

        var rates = await _sut.GetRates(CancellationToken.None);

        rates.Should().BeNull();
        _logger.Collector.LatestRecord.Should().NotBeNull();
        _logger.Collector.LatestRecord.Level.Should().Be(LogLevel.Error);
        _logger.Collector.LatestRecord.Message.Should().Contain("status code InternalServerError");
    }
    
    [Fact]
    public async Task GetRates_Should_Handle_NotFound()
    {
        _server.Given(Request.Create().WithPath("/exrates/daily").UsingGet())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.NotFound)
            );

        var rates = await _sut.GetRates(CancellationToken.None);

        rates.Should().BeNull();
        _logger.Collector.LatestRecord.Should().NotBeNull();
        _logger.Collector.LatestRecord.Level.Should().Be(LogLevel.Error);
        _logger.Collector.LatestRecord.Message.Should().Contain("status code NotFound");
    }
}