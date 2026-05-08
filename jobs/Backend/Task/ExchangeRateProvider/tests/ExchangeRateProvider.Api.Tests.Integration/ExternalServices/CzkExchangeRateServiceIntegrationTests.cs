using ExchangeRateProvider.Domain.ValueObjects;
using ExchangeRateProvider.Infrastructure.ExternalServices.CZK;
using ExchangeRateProvider.Infrastructure.Policies;
using LazyCache;
using Microsoft.Extensions.Logging.Abstractions;
using Polly.Registry;
using Shouldly;

namespace ExchangeRateProvider.Api.Tests.Integration.ExternalServices;

public class CzkExchangeRateServiceIntegrationTests
{
    [Fact]
    public async Task GetExchangeRatesAsync_WithRealCnbApi_ReturnsRates()
    {
        // Arrange
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.cnb.cz"),
            Timeout = TimeSpan.FromSeconds(30)
        };

        var policyRegistry = new PolicyRegistry().AddBasicRetryPolicy();
        var cache = new CachingService();
        var apiClientLogger = NullLogger<CzkApiClient>.Instance;
        var mapperLogger = NullLogger<CzkExchangeRateMapper>.Instance;

        var apiClient = new CzkApiClient(httpClient, policyRegistry, cache, apiClientLogger);
        var mapper = new CzkExchangeRateMapper(mapperLogger);
        var service = new CzkExchangeRateService(apiClient, mapper);

        // Act
        var rates = await service.GetExchangeRatesAsync(new Currency("CZK"), CancellationToken.None);

        // Assert
        rates.ShouldNotBeNull();
        rates.ShouldNotBeEmpty();
        rates.ShouldAllBe(r => r.BaseCurrency.Code == "CZK");
        rates.ShouldAllBe(r => r.Rate > 0);

        // Verify some expected currencies
        rates.ShouldContain(r => r.QuoteCurrency.Code == "EUR");
        rates.ShouldContain(r => r.QuoteCurrency.Code == "USD");
    }
}