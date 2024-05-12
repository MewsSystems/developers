using FluentAssertions;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using ExchangeRateUpdater.ApiTests.Setup;
using RichardSzalay.MockHttp;
using System.Text.Json;
using ExchangeRateUpdater.ApiTests;
using ExchangeRateUpdater.Application.Features.ExchangeRates.GetByCurrency;

namespace ExchangeRateProvider.ApiTests.GetExchangeRates;

[Collection("ExchangeRateUpdaterTests")]
public class GivenNoForDateWhenGetExchangeRatesIsCalled(ExchangeRateUpdaterApplicationFactory factory) : IAsyncLifetime
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
            .Respond(HttpStatusCode.OK, new StringContent(StubbedApiResponsesCNBExchangeRateService.GetDailyExchangeRates200OkResponse));

        _httpClient = factory.HttpClient;
    }

    [Fact]
    public async void Then200OkResponseReturnedWithExchangeRateInCorrectFormat()
    {
        var getExchangeRatesQuery = new GetExchangeRatesByCurrencyQuery(CurrencyCodes: ["EUR"], null);
        var json = JsonSerializer.Serialize(getExchangeRatesQuery);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _httpResponse = await _httpClient.PostAsync("/exchangeRates", content);
        _httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var exchangeRatesResponse = await _httpResponse.Content.ReadFromJsonAsync<GetExchangeRatesByCurrencyQueryResponse>();

        Assert.Multiple(
            () => exchangeRatesResponse!.ExchangeRates.Should().NotBeNullOrEmpty(),
            () => exchangeRatesResponse!.ExchangeRates.FirstOrDefault()!.SourceCurrency.Code.Should().Be("EUR"),
            () => exchangeRatesResponse!.ExchangeRates.FirstOrDefault()!.TargetCurrency.Code.Should().Be("CZK"),
            () => exchangeRatesResponse!.ExchangeRates.FirstOrDefault()!.Value.Should().Be(25.005m),
            () => exchangeRatesResponse!.ExchangeRates.FirstOrDefault()!.ToString().Should().Be("EUR/CZK=25.005"));
    }
}
