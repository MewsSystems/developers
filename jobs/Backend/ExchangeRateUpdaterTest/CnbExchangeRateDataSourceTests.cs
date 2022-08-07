using ExchangeRateUpdater.ExchangeRateDataSources;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRateUpdaterTest
{
    public class CnbExchangeRateDataSourceTests
    {
        [Fact]
        public async Task GetData_Success()
        {
            string content = "Hello";

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(content)
                });

            var source = new CnbExchangeRateDataSource(new HttpClient(mockMessageHandler.Object));
            var result = await source.GetDataAsync();

            Assert.NotNull(result);
            Assert.True(result.Length == 5);
        }

        [Fact]
        public async Task GetData_Exception()
        {
            string content = "Hello";

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(content)
                });

            var source = new CnbExchangeRateDataSource(new HttpClient(mockMessageHandler.Object));
            Assert.ThrowsAsync<Exception>(() => source.GetDataAsync());
        }
    }
}
