namespace ExchangeReaderUpdater.Test
{
    using ExchangeRateUpdater.Exceptions;
    using ExchangeRateUpdater.ExchangeClients;
    using ExchangeRateUpdater.Settings;
    using FluentAssertions;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Extensions.Options;
    using Moq;
    using Moq.Protected;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class CnbExchangeRateClientTests
    {
        [Fact]
        public async Task getDailyRatesAsync_returnsContent_whenResponseIsSuccess()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("daily content")
            };

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response)
                .Verifiable();

            using var httpClient = new HttpClient(handlerMock.Object);

            var optionsMock = new Mock<IOptions<ExchangeRateProviderSettings>>();
            optionsMock.SetupGet(o => o.Value).Returns(new ExchangeRateProviderSettings { CnbUrl = "http://test" });

            var client = new CnbExchangeRateClient(httpClient, optionsMock.Object, NullLogger<CnbExchangeRateClient>.Instance);

            var result = await client.GetDailyRatesAsync(CancellationToken.None);

            result.Should().Be("daily content");

            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task getDailyRatesAsync_throwsExchangeRateUpdateException_onNonSuccessStatus()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("error")
            };

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response)
                .Verifiable();

            using var httpClient = new HttpClient(handlerMock.Object);

            var optionsMock = new Mock<IOptions<ExchangeRateProviderSettings>>();
            optionsMock.SetupGet(o => o.Value).Returns(new ExchangeRateProviderSettings { CnbUrl = "http://test" });

            var client = new CnbExchangeRateClient(httpClient, optionsMock.Object, NullLogger<CnbExchangeRateClient>.Instance);

            Func<Task> act = async () => await client.GetDailyRatesAsync(CancellationToken.None);

            await act.Should().ThrowAsync<ExchangeRateUpdateException>()
                .Where(ex => ex.InnerException is HttpRequestException);
        }

        [Fact]
        public async Task getDailyRatesAsync_wrapsHandlerHttpRequestException()
        {
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("network error"))
                .Verifiable();

            using var httpClient = new HttpClient(handlerMock.Object);

            var optionsMock = new Mock<IOptions<ExchangeRateProviderSettings>>();
            optionsMock.SetupGet(o => o.Value).Returns(new ExchangeRateProviderSettings { CnbUrl = "http://test" });

            var client = new CnbExchangeRateClient(httpClient, optionsMock.Object, NullLogger<CnbExchangeRateClient>.Instance);

            Func<Task> act = async () => await client.GetDailyRatesAsync(CancellationToken.None);

            await act.Should().ThrowAsync<ExchangeRateUpdateException>()
                .Where(ex => ex.InnerException is HttpRequestException && ex.InnerException.Message.Contains("network error"));
        }

        [Fact]
        public async Task getDailyRatesAsync_propagatesCancellation()
        {
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new TaskCanceledException())
                .Verifiable();

            using var httpClient = new HttpClient(handlerMock.Object);

            var optionsMock = new Mock<IOptions<ExchangeRateProviderSettings>>();
            optionsMock.SetupGet(o => o.Value).Returns(new ExchangeRateProviderSettings { CnbUrl = "http://test" });

            var client = new CnbExchangeRateClient(httpClient, optionsMock.Object, NullLogger<CnbExchangeRateClient>.Instance);

            Func<Task> act = async () => await client.GetDailyRatesAsync(new CancellationToken(true));

            await act.Should().ThrowAsync<TaskCanceledException>();
        }
    }
}
