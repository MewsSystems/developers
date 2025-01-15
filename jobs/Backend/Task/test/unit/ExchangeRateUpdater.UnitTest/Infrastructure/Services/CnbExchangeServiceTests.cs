using ExchangeRateUpdater.Infrastructure.Services.Providers;
using ExchangeRateUpdater.UnitTest.Fixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.UnitTest.Infrastructure.Services
{
    public class CnbExchangeServiceTests
    {
        private readonly IConfiguration _configuration;
        private readonly CnbFixtures _cnbFixtures;

        public CnbExchangeServiceTests()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"ExchangeRateProviders:Cnb:UrlExchangeRate", "exchange/v1"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(initialData: inMemorySettings!)
                .Build();

            _cnbFixtures = new CnbFixtures();
        }

        [Fact]
        public async Task GetExchangeRatesByDateAsync_Ok()
        {
            // Arrange
            var mockHttpClient = HttpClientMock.GetMockHttpClient(_cnbFixtures.HttpCnbOkResponse);
            var _cnbService = new CnbExchangeService(mockHttpClient, _configuration);

            // Act
            var cnbResponse = await _cnbService.GetExchangeRatesByDateAsync(DateTime.Today, CancellationToken.None);

            // Assert
            cnbResponse.Should().NotBeNull();
        }
    }
}
