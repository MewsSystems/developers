using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Data;
using ExchangeRateUpdater.Infrastructure.Http;
using ExchangeRateUpdater.Core.Interfaces;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Tests.Integration
{
    public class CnbApiClientIntegrationTests
    {
        [Fact]
        [Trait("Category", "Integration")] // An attribute to mark this as a slow integration test
        public async Task GetLatestExchangeRatesAsync_WhenCalled_ConnectsToCnbAndFetchesData()
        {
            var options = Options.Create(new CnbApiOptions());

            var httpClient = new HttpClient();
            var mockLogger = new Mock<ILogger<CnbApiClient>>();

            var apiClient = new CnbApiClient(httpClient, mockLogger.Object, options);


            var result = await apiClient.GetLatestExchangeRatesAsync();

            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result));
        }
    }
}