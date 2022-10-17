using System.Net;
using ExchangeRateUpdater.Clients.Cnb.Parsers;
using ExchangeRateUpdater.Clients.Cnb.Responses;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace ExchangeRateUpdater.Clients.Cnb.Tests;

public class CnbClientTests
{
    private CnbClient _cnbClient;
    private Mock<HttpMessageHandler> _httpMessageHandler;
    private readonly Mock<ILogger<CnbClientResponseParser>> _logger;
    private readonly Uri _url;

    public CnbClientTests()
    {
        _logger = new Mock<ILogger<CnbClientResponseParser>>();
        _url = new Uri("http://localhost/");
    }

    [Fact]
    public async Task Given_request_when_response_is_empty_then_it_should_throw_Exception()
    {
        //Arrange
        var stream = new MemoryStream();

        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StreamContent(stream)
        };

        _httpMessageHandler = new Mock<HttpMessageHandler>();
        _httpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var client = new HttpClient(_httpMessageHandler.Object)
        {
            BaseAddress = _url
        };
        _cnbClient = new CnbClient(client, new CnbClientResponseParser(_logger.Object));

        //Act
        Func<Task<ExchangeRatesResponse>> act = async () => await _cnbClient.GetExchangeRatesAsync();

        // Assert
        await act.Should().ThrowExactlyAsync<Exception>().WithMessage("Information is missing.");
    }

    [Fact]
    public async Task Given_request_when_title_is_invalid_then_it_should_throw_Exception()
    {
        //Arrange
        var stream = new MemoryStream();
        var sw = new StreamWriter(stream);
        await sw.WriteLineAsync("title");
        await sw.FlushAsync();
        stream.Position = 0;

        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StreamContent(stream)
        };

        _httpMessageHandler = new Mock<HttpMessageHandler>();
        _httpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var client = new HttpClient(_httpMessageHandler.Object)
        {
            BaseAddress = _url
        };
        _cnbClient = new CnbClient(client, new CnbClientResponseParser(_logger.Object));

        //Act
        Func<Task<ExchangeRatesResponse>> act = async () => await _cnbClient.GetExchangeRatesAsync();

        // Assert
        await act.Should().ThrowExactlyAsync<Exception>().WithMessage("Invalid date.");
    }
    
    [Fact]
    public async Task Given_request_when_column_info_is_empty_then_it_should_throw_Exception()
    {
        //Arrange
        var stream = new MemoryStream();
        var sw = new StreamWriter(stream);
        await sw.WriteLineAsync("14 Oct 2022 #200");
        await sw.FlushAsync();
        stream.Position = 0;

        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StreamContent(stream)
        };

        _httpMessageHandler = new Mock<HttpMessageHandler>();
        _httpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var client = new HttpClient(_httpMessageHandler.Object)
        {
            BaseAddress = _url
        };
        _cnbClient = new CnbClient(client, new CnbClientResponseParser(_logger.Object));

        //Act
        Func<Task<ExchangeRatesResponse>> act = async () => await _cnbClient.GetExchangeRatesAsync();

        // Assert
        await act.Should().ThrowExactlyAsync<Exception>().WithMessage("Column is missing.");
    }

    [Fact]
    public async Task Given_request_when_valid_response_is_valid_then_it_should_map_properly()
    {
        //Arrange
        var stream = new MemoryStream();
        var sw = new StreamWriter(stream);
        await sw.WriteLineAsync("14 Oct 2022 #200");
        await sw.WriteLineAsync("Country|Currency|Amount|Code|Rate");
        await sw.WriteLineAsync("Australia|dollar|1|AUD|15.867");

        await sw.FlushAsync();
        stream.Position = 0;

        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StreamContent(stream)
        };

        _httpMessageHandler = new Mock<HttpMessageHandler>();
        _httpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var client = new HttpClient(_httpMessageHandler.Object)
        {
            BaseAddress = _url
        };

        _cnbClient = new CnbClient(client, new CnbClientResponseParser(_logger.Object));

        //Act
        var response = await _cnbClient.GetExchangeRatesAsync();

        // Assert
        response.CurrentDate.Should().Be(new DateTime(2022, 10, 14, 0, 0, 0));
        response.ExchangeRates.Count().Should().Be(1);
        response.ExchangeRates.First().Rate.Should().Be((decimal)15.867);
        response.ExchangeRates.First().Code.Should().Be("AUD");
        response.ExchangeRates.First().Country.Should().Be("Australia");
        response.ExchangeRates.First().Currency.Should().Be("dollar");
        response.ExchangeRates.First().Amount.Should().Be(1);
    }
}