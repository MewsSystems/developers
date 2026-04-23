using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using ExchangeRateUpdater.Api.Infrastructure.Providers.Cnb;
using ExchangeRateUpdater.Api.Infrastructure.Providers.Cnb.Models;
using ExchangeRateUpdater.Api.Tests.Helpers;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ExchangeRateUpdater.Api.Tests.Infrastructure.CnbApi;

public class CnbApiClientTests
{
    private readonly ILogger<CnbApiClient> _logger = Substitute.For<ILogger<CnbApiClient>>();

    private CnbApiClient CreateClient(HttpStatusCode statusCode, object? responseBody = null)
    {
        HttpContent? content = responseBody is not null
            ? new StringContent(JsonSerializer.Serialize(responseBody), Encoding.UTF8, MediaTypeNames.Application.Json)
            : null;

        var handler = new FakeHttpMessageHandler(statusCode, content);
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://api.cnb.cz") };

        return new CnbApiClient(httpClient, _logger);
    }

    private static FakeHttpMessageHandler CreateHandler(HttpStatusCode statusCode, object? responseBody = null)
    {
        HttpContent? content = responseBody is not null
            ? new StringContent(JsonSerializer.Serialize(responseBody), Encoding.UTF8, MediaTypeNames.Application.Json)
            : null;

        return new FakeHttpMessageHandler(statusCode, content);
    }

    public class GetDailyRatesAsync : CnbApiClientTests
    {
        [Theory]
        [InlineData(null, false)]
        [InlineData("2025-03-15", true)]
        public async Task GetDailyRatesAsync_BuildsCorrectUrl(string? dateString, bool shouldContainDate)
        {
            // Arrange
            var date = dateString is not null ? DateOnly.Parse(dateString) : (DateOnly?)null;
            var responseBody = new CnbExRateDailyResponse([]);
            var handler = CreateHandler(HttpStatusCode.OK, responseBody);
            var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://api.cnb.cz") };
            var client = new CnbApiClient(httpClient, _logger);

            // Act
            await client.GetDailyRatesAsync(date, CancellationToken.None);

            // Assert
            Assert.NotNull(handler.LastRequest);
            var url = handler.LastRequest!.RequestUri!.ToString();
            Assert.Contains("/cnbapi/exrates/daily", url);
            Assert.Contains("lang=EN", url);
            
            if (shouldContainDate)
                Assert.Contains($"date={dateString}", url);
            else
                Assert.DoesNotContain("date=", url);
        }

        [Fact]
        public async Task GetDailyRatesAsync_WithSuccessResponse_DeserializesRates()
        {
            // Arrange
            var responseBody = new CnbExRateDailyResponse(
            [
                new CnbExRateDaily("2025-06-15", 115, "USA", "dollar", 1, "USD", 23.45m)
            ]);

            var client = CreateClient(HttpStatusCode.OK, responseBody);

            // Act
            var result = await client.GetDailyRatesAsync(null, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            var rates = result.Value.Rates;
            var rate = Assert.Single(rates);
            Assert.Equal("USD", rate.CurrencyCode);
            Assert.Equal(23.45m, rate.Rate);
            Assert.Equal(1, rate.Amount);
        }

        [Fact]
        public async Task GetDailyRatesAsync_WhenHttpRequestFails_ReturnsUnavailableError()
        {
            // Arrange
            var client = CreateClient(HttpStatusCode.InternalServerError);

            // Act
            var result = await client.GetDailyRatesAsync(null, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("ExchangeRate.SourceUnavailable", result.Error.Code);
        }

        [Fact]
        public async Task GetDailyRatesAsync_WithInvalidJson_ReturnsUnavailableError()
        {
            // Arrange
            var handler = new FakeHttpMessageHandler(
                HttpStatusCode.OK,
                new StringContent("not json", Encoding.UTF8, MediaTypeNames.Application.Json));
            var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://api.cnb.cz") };
            var client = new CnbApiClient(httpClient, _logger);

            // Act
            var result = await client.GetDailyRatesAsync(null, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("ExchangeRate.SourceUnavailable", result.Error.Code);
        }
    }
}
