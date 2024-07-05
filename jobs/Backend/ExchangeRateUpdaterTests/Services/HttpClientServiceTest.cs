using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdaterTests.Services
{
    public class HttpClientServiceTests
    {
        [Fact]
        public async Task FetchDataAsync_ReturnsContent_WhenSuccessful()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            string testUrl = "http://test.com/data";
            string expectedResponse = "{\"key\":\"value\"}";
            mockHttp.When(HttpMethod.Get, testUrl)
                    .Respond("application/json", expectedResponse);

            var httpClient = new HttpClient(mockHttp);
            var httpClientService = new HttpClientService(httpClient);

            // Act
            var result = await httpClientService.FetchDataAsync(testUrl);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task FetchDataAsync_RetriesAndFails_WhenServerError()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            string testUrl = "http://test.com/error";
            int attemptCount = 0;

            mockHttp.When(HttpMethod.Get, testUrl)
                    .Respond(_ =>
                    {
                        attemptCount++;
                        throw new HttpRequestException("Simulated network error");
                    });

            var httpClient = new HttpClient(mockHttp);
            var httpClientService = new HttpClientService(httpClient);

            // Act
            var result = await httpClientService.FetchDataAsync(testUrl);

            // Assert
            Assert.Null(result);
            Assert.Equal(4, attemptCount);
        }

        [Fact]
        public async Task FetchDataAsync_HandlesApiErrorResponseCorrectly()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            string testUrl = "http://test.com/notfound";
            var apiErrorResponse = new ApiErrorResponse { ErrorCode = "NOT_FOUND", Description = "Not found error" };
            string jsonResponse = JsonConvert.SerializeObject(apiErrorResponse);

            mockHttp.When(HttpMethod.Get, testUrl)
                    .Respond(HttpStatusCode.NotFound, "application/json", jsonResponse);

            var httpClient = new HttpClient(mockHttp);
            var httpClientService = new HttpClientService(httpClient);

            // Act
            var result = await httpClientService.FetchDataAsync(testUrl);

            // Assert
            Assert.Null(result);
        }
    }
}
