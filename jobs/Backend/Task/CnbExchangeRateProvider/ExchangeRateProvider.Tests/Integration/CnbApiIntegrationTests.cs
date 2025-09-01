using ExchangeRateProvider.Domain.Entities;
using ExchangeRateProvider.Infrastructure;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace ExchangeRateProvider.Tests.Integration
{
    /// <summary>
    /// integration tests for CNB API external contract.
    /// </summary>
    public class CnbApiIntegrationTests
    {
        [Fact]
        public async Task CnbExchangeRateProvider_FetchesRealRates_WithValidCurrencies()
        {
            // Integration test: Actual API call to CNB
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://api.cnb.cz");
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateClient("CnbExchangeRateProvider")).Returns(httpClient);
            var provider = new CnbExchangeRateProvider(httpClientFactoryMock.Object, NullLogger<CnbExchangeRateProvider>.Instance);

            var requestedCurrencies = new[]
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("GBP")
            };

            // Act - Real API call
            var rates = await provider.GetExchangeRatesAsync(requestedCurrencies);

            // Assert - Business rules
            Assert.NotEmpty(rates);
            Assert.All(rates, rate => Assert.Equal("CZK", rate.TargetCurrency.Code));
            Assert.All(rates, rate => Assert.Contains(rate.SourceCurrency.Code, new[] { "USD", "EUR", "GBP" }));
            Assert.All(rates, rate => Assert.True(rate.Value > 0));
        }

        [Fact]
        public async Task CnbExchangeRateProvider_HandlesNetworkFailure_Gracefully()
        {
            // Integration test: Network failure scenario
            // High-risk failure: Network issues
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://api.cnb.cz");
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateClient("CnbExchangeRateProvider")).Returns(httpClient);
            var provider = new CnbExchangeRateProvider(httpClientFactoryMock.Object, NullLogger<CnbExchangeRateProvider>.Instance);

            // Configure client to timeout quickly for test
            httpClient.Timeout = TimeSpan.FromMilliseconds(1);

            var requestedCurrencies = new[] { new Currency("USD") };

            // Act & Assert - Should throw on network timeout (TaskCanceledException)
            await Assert.ThrowsAsync<TaskCanceledException>(
                () => provider.GetExchangeRatesAsync(requestedCurrencies));
        }

        [Fact]
        public async Task CnbExchangeRateProvider_FiltersRequestedCurrencies_Correctly()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://api.cnb.cz");
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateClient("CnbExchangeRateProvider")).Returns(httpClient);
            var provider = new CnbExchangeRateProvider(httpClientFactoryMock.Object, NullLogger<CnbExchangeRateProvider>.Instance);

            var requestedCurrencies = new[] { new Currency("USD") };

            // Act
            var rates = await provider.GetExchangeRatesAsync(requestedCurrencies);

            // Assert - Should only contain requested currencies
            Assert.All(rates, rate => Assert.Equal("USD", rate.SourceCurrency.Code));
        }
    }
}