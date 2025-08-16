using ExchangeRateUpdater.Application.Features.ExchangeRates.GetByCurrency;
using ExchangeRateUpdater.ApiTests;
using FluentAssertions;
using RichardSzalay.MockHttp;
using System.Net;
using System.Text;
using System.Text.Json;
using ExchangeRateUpdater.ApiTests.Setup;

namespace ExchangeRateProvider.ApiTests.GetExchangeRates;

[Collection("ExchangeRateUpdaterTests")]
public class GivenExternalExchangeRateServiceErrorWhenGetExchangeRatesIsCalled(ExchangeRateUpdaterApplicationFactory factory) : IAsyncLifetime
{
    private HttpResponseMessage _httpResponse = null!;
    private HttpClient _httpClient = null!;
    private DateTime _queryDate = DateTime.UtcNow;

    public Task DisposeAsync() => Task.CompletedTask;

    public async Task InitializeAsync()
    {
        await Task.Yield();
        factory.ExchangeRateServiceMockHttpHandler.Clear();
        factory.ExchangeRateServiceMockHttpHandler
            .When(HttpMethod.Get, $"https://api.cnb.cz/cnbapi/exrates/daily?date={_queryDate.ToString("yyyy-MM-dd")}&lang=EN")
            .Respond(HttpStatusCode.InternalServerError, new StringContent(StubbedApiResponsesCNBExchangeRateService.GetDailyExchangeRates500InternalServerError));

        _httpClient = factory.HttpClient;
    }


    [Fact]
    public async void Then500InternalServerErrorResponseReturned()
    {
        var getExchangeRatesQuery = new GetExchangeRatesByCurrencyQuery(CurrencyCodes: ["EUR"], ForDate: _queryDate);
        var json = JsonSerializer.Serialize(getExchangeRatesQuery);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _httpResponse = await _httpClient.PostAsync("/exchangeRates", content);
        _httpResponse.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}
