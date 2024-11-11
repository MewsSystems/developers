using Moq.Protected;
using Moq;
using System.Net;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Models;
using FluentAssertions;

namespace ExchangeRateUpdater.Tests
{
    public class FxRateServiceTests
    {
        private IFxRateService _fxRateService;
        private readonly Mock<IHttpClientFactory> _httpFactory;
        private readonly Mock<ILogger<CNBFxRateService>> _logger;

        public FxRateServiceTests()
        {
            _httpFactory = new Mock<IHttpClientFactory>();
            _logger = new Mock<ILogger<CNBFxRateService>>();
        }

        [Fact]
        public async Task GetExchangeRatesForCZK_ShouldReturnTheApiFxRates_WhenTheCNBApiIsReacheable()
        {
            _fxRateService = new CNBFxRateService(_httpFactory.Object, _logger.Object);
            var httpClient = CreateHttpClientMock(HttpStatusCode.OK);
            _httpFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            var expectedFxRates = new List<FxRate>
            {
                new() { ValidFor = DateTime.Parse("2024-10-01"), Order = 191, Country = "Australia", Currency = "dollar", Amount = 1, CurrencyCode = "AUD", Rate = 15.759 },
                new() { ValidFor = DateTime.Parse("2024-10-01"), Order = 191, Country = "Canada", Currency = "dollar", Amount = 1, CurrencyCode = "CAD", Rate = 16.873 },
                new() { ValidFor = DateTime.Parse("2024-10-01"), Order = 191, Country = "EMU", Currency = "euro", Amount = 1, CurrencyCode = "EUR", Rate = 25.275 },
                new() { ValidFor = DateTime.Parse("2024-10-01"), Order = 191, Country = "United Kingdom", Currency = "pound", Amount = 1, CurrencyCode = "GBP", Rate = 30.389 },
                new() { ValidFor = DateTime.Parse("2024-10-01"), Order = 191, Country = "USA", Currency = "dollar", Amount = 1, CurrencyCode = "USD", Rate = 22.81 }
            };

            var result = await _fxRateService.GetFxRatesAsync(DateTime.UtcNow, "EN", default);

            result.Should().BeEquivalentTo(expectedFxRates);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadGateway)]
        [InlineData(HttpStatusCode.NotFound)]
        public async Task GetExchangeRatesForCZK_ShouldLogWarningAndReturnEmptyList_WhenApiIsReachableButNonSuccessStatusCodeIsReturned(HttpStatusCode code)
        {
            _fxRateService = new CNBFxRateService(_httpFactory.Object, _logger.Object);
            var httpClient = CreateHttpClientMock(code);
            _httpFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var result = await _fxRateService.GetFxRatesAsync(DateTime.UtcNow, "EN", default);

            result.Should().BeEmpty();
            _logger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("The rates could not be retrieved from the CNB exchange rates API")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }

        private HttpClient CreateHttpClientMock(HttpStatusCode returnedCode)
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = returnedCode,
                    Content = new StringContent("{\"rates\":[{\"validFor\":\"2024-10-01\",\"order\":191,\"country\":\"Australia\",\"currency\":\"dollar\",\"amount\":1,\"currencyCode\":\"AUD\",\"rate\":15.759},{\"validFor\":\"2024-10-01\",\"order\":191,\"country\":\"Canada\",\"currency\":\"dollar\",\"amount\":1,\"currencyCode\":\"CAD\",\"rate\":16.873},{\"validFor\":\"2024-10-01\",\"order\":191,\"country\":\"EMU\",\"currency\":\"euro\",\"amount\":1,\"currencyCode\":\"EUR\",\"rate\":25.275},{\"validFor\":\"2024-10-01\",\"order\":191,\"country\":\"United Kingdom\",\"currency\":\"pound\",\"amount\":1,\"currencyCode\":\"GBP\",\"rate\":30.389},{\"validFor\":\"2024-10-01\",\"order\":191,\"country\":\"USA\",\"currency\":\"dollar\",\"amount\":1,\"currencyCode\":\"USD\",\"rate\":22.81}]}")
                });

            var client = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://test.com")
            };

            return client;
        }
    }
}
