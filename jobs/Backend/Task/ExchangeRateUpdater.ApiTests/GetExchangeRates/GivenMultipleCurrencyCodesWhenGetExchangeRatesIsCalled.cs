using FluentAssertions;
using ExchangeRateUpdater.ApiTests.Setup;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ExchangeRateUpdater.ApiTests;
using ExchangeRateUpdater.Application.Features.ExchangeRates.GetByCurrency;

namespace ExchangeRateProvider.ApiTests.GetExchangeRates;

[Collection("ExchangeRateUpdaterTests")]
public class GivenMultipleCurrencyCodesWhenGetExchangeRatesIsCalled(ExchangeRateUpdaterApplicationFactory factory) : IAsyncLifetime
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
    public async void Then200OkResponseReturnedWithCorrectCountOfExchangRateAndCorrectFormat()
    {
        var getExchangeRatesQuery = new GetExchangeRatesByCurrencyQuery(CurrencyCodes: ["EUR", "USD", "GBP"], ForDate: _queryDate);
        var json = JsonSerializer.Serialize(getExchangeRatesQuery);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _httpResponse = await _httpClient.PostAsync("/exchangeRates", content);
        _httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var exchangeRatesResponse = await _httpResponse.Content.ReadFromJsonAsync<GetExchangeRatesByCurrencyQueryResponse>();

        exchangeRatesResponse!.ExchangeRates.Count().Should().Be(getExchangeRatesQuery.CurrencyCodes.Count());

        var eurRate = exchangeRatesResponse!.ExchangeRates.First(r => r.SourceCurrency.Code == "EUR");
        var usdRate = exchangeRatesResponse!.ExchangeRates.First(r => r.SourceCurrency.Code == "USD");
        var gbpRate = exchangeRatesResponse!.ExchangeRates.First(r => r.SourceCurrency.Code == "GBP");

        Assert.Multiple(
        () => eurRate.ToString().Should().Be("EUR/CZK=25.005"),
        () => usdRate.ToString().Should().Be("USD/CZK=23.223"),
        () => gbpRate.ToString().Should().Be("GBP/CZK=29.136"));
    }
}
