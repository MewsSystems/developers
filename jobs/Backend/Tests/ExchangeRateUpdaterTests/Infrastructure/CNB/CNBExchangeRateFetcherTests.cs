using System.Net;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure.CNB;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Polly.CircuitBreaker;

namespace ExchangeRateUpdaterTests.Infrastructure.CNB
{
    public class CNBExchangeRateFetcherTests
    {
        private readonly Mock<HttpMessageHandler> mockHandler = new(MockBehavior.Strict);
        private readonly Mock<IExchangeRateParser> mockParser = new();
        private readonly Mock<ILogger<CNBExchangeRateFetcher>> mockLogger = new();
        private readonly HttpClient httpClient;
        private readonly CNBExchangeRateFetcher sut;

        public CNBExchangeRateFetcherTests()
        {
            httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://cnb.cz/")
            };
            sut = new CNBExchangeRateFetcher(httpClient, mockParser.Object, mockLogger.Object);
        }

        [Fact]
        public async Task GivenValidResponse_WhenGetExchangeRates_ThenReturnsParsedRates()
        {
            // Arrange
            var responseContent = "test-data";
            var expectedRates = new List<ExchangeRate> { new(new Currency("CZK"), new Currency("USD"), 1.23m) };
            var parserResult = (new ExchangeRateMetadata(DateTime.Today, 1), expectedRates);

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri!.PathAndQuery == "/daily.txt"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(responseContent)
                });
            mockParser.Setup(p => p.Parse(responseContent)).Returns(parserResult);

            // Act
            var result = await sut.GetExchangeRates();

            // Assert
            result.Should().BeEquivalentTo(expectedRates);
        }

        [Fact]
        public async Task GivenHttpError_WhenGetExchangeRates_ThenThrows()
        {
            // Arrange
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri!.PathAndQuery == "/daily.txt"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));

            // Act
            var act = async () => await sut.GetExchangeRates();

            // Assert
            await act.Should().ThrowAsync<HttpRequestException>();
        }

        [Fact]
        public async Task GivenBrokenCircuitException_WhenGetExchangeRates_ThenReturnsEmpty()
        {
            // Arrange
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri!.PathAndQuery == "/daily.txt"),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new BrokenCircuitException());

            // Act
            var result = await sut.GetExchangeRates();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
