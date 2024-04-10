using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ExchangeRates.Api.Tests.Integration.Controllers
{
    public class ExchangeRatesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ExchangeRatesControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/exchange-rates")]
        public async Task GetExchangeRates_ReturnsSuccess_WithoutDayProvided(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("/exchange-rates")]
        public async Task GetExchangeRates_ReturnsNotEmpty_WithoutDayProvided(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);
            var contents = await response.Content.ReadAsStringAsync();

            // Assert
            contents.Should().NotBeEmpty();
        }
    }
}