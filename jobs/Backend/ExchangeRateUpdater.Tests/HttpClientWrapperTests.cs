using ExchangeRateUpdater.Services;
using RichardSzalay.MockHttp;

namespace ExchangeRateUpdater.Tests
{
    public class HttpClientWrapperTests
    {
        [Fact]
        public async Task GetStringAsync_ReturnsExpectedResponse()
        {
            // Arrange
            var expectedResponse = "Expected response data";
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://test.com")
                .Respond("text/plain", expectedResponse);

            var httpClient = mockHttp.ToHttpClient();
            var clientWrapper = new HttpClientWrapper(httpClient);

            // Act
            var response = await clientWrapper.GetStringAsync("https://test.com");

            // Assert
            Assert.Equal(expectedResponse, response);
        }

        [Fact]
        public async Task GetStringAsync_ThrowsHttpRequestException()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://test.com")
                .Throw(new HttpRequestException("Request error"));

            var httpClient = mockHttp.ToHttpClient();
            var clientWrapper = new HttpClientWrapper(httpClient);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => clientWrapper.GetStringAsync("https://test.com"));
        }
    }
}
