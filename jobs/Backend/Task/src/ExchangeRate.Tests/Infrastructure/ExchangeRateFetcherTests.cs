using System.Net;
using AutoFixture;
using ExchangeRate.Infrastructure.Cnb;
using ExchangeRate.Infrastructure.Cnb.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.Protected;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ExchangeRate.Tests.Infrastructure;

public class ExchangeRateFetcherTests : TestsBase
{
    private Mock<IHttpClientFactory> _mockHttpClientFactory = null!;
    private ExchangeRateFetcher _exchangeRateFetcher = null!;
    private Mock<HttpMessageHandler> _mockHttpMessageHandler = null!;
    
    [SetUp]
    public void SetUp()
    {
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        _exchangeRateFetcher = new ExchangeRateFetcher(_mockHttpClientFactory.Object, new NullLogger<ExchangeRateFetcher>());
    }

    [Test]
    public async Task GetDailyExchangeRates_ReturnsExchangeRates()
    {
        // Arrange
        var expectedDate = DateTime.Today; 
        var (fakeRates, fakeRatesJson ) = GenerateFakeApiResponse(expectedDate);

        MockHttpClient(fakeRatesJson);
        
        // Act
        var result = (await _exchangeRateFetcher.GetDailyExchangeRates(Fixture.Create<string>())).ToList();
        
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result.Should().BeEquivalentTo(fakeRates.Rates);
    }
    
    [Test]
    public async Task GetDailyExchangeRates_Throws_WhenReceivedJsonIsNotSerializable()
    {
        // Arrange
        var invalidJson = "invalid json string";

        MockHttpClient(invalidJson);
        
        // Act/ Assert
       await _exchangeRateFetcher.Invoking(x => x.GetDailyExchangeRates(Fixture.Create<string>()))
           .Should()
           .ThrowAsync<Exception>();
    }

    private void MockHttpClient(string fakeRatesJson)
    {
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get), // Ensure the method is GET
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(fakeRatesJson)
            });
    }

    private (ExchangeRates, string) GenerateFakeApiResponse(DateTime validFor)
    {
        var fakeRates = Fixture.Build<ExchangeRates>()
            .With(x => x.Rates, () => GenerateExchangeRates(validFor))
            .Create();

        var fakeRatesJson = JsonSerializer.Serialize(fakeRates);

        return (fakeRates, fakeRatesJson);
    }
}