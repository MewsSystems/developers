using ExchangeRateUpdater.Application.Features.ExchangeRates.GetByCurrency;
using FluentAssertions;
using System.Net;
using System.Text;
using ExchangeRateUpdater.ApiTests.Setup;
using System.Text.Json;

namespace ExchangeRateProvider.ApiTests.GetExchangeRates;

[Collection("ExchangeRateUpdaterTests")]
public class GivenInvalidApiKeyWhenGetExchangeRatesIsCalled(ExchangeRateUpdaterApplicationFactory factory) : IAsyncLifetime
{
    private HttpResponseMessage _httpResponse = null!;
    private HttpClient _httpClient = null!;
    private DateTime _queryDate = DateTime.UtcNow;

    public Task DisposeAsync() => Task.CompletedTask;

    public async Task InitializeAsync()
    {
        await Task.Yield();
        _httpClient = factory.CreateClient();
        _httpClient.DefaultRequestHeaders.Remove("X-Api-Key");
    }


    [Fact]
    public async void Then401UnauthorizedReturned()
    {
        var getExchangeRatesQuery = new GetExchangeRatesByCurrencyQuery(CurrencyCodes: ["EUR"], ForDate: _queryDate);
        var json = JsonSerializer.Serialize(getExchangeRatesQuery);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _httpResponse = await _httpClient.PostAsync("/exchangeRates", content);
        _httpResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
