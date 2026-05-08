using ExchangeRateUpdater.API.Dtos;
using System.Net;
using System.Net.Http.Json;
using Xunit;
using Assert = Xunit.Assert;

namespace ExchangeRateUpdater.IntegrationTests.UseCase
{
    public class ExchangeRateApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ExchangeRateApiTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetExchangeRates_ShouldReturnCorrectResult_WhenEverythingIsOk()
        {
            // Arrange
            var url = "/api/exchangeRates?currencies=EUR";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<List<ExchangeRateResponseDto>>();

            Assert.NotNull(result);
            Assert.Equal(1, result.Count);

            Assert.True(result.Any(r =>
                r.TargetCurrencyCode == "EUR" &&
                r.SourceCurrencyCode == "CZK" &&
                r.Rate == 24.0m), "EUR/CZK not found");
        }
    }
}
