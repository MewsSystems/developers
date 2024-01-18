using ExchangeRateUpdater.Config;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;

namespace ExchangeRateUpdater.Tests.Fixtures;

public class ExchangeRateData : IDisposable
{
    private readonly string _apiJsonResponse;
    private readonly MockHttpMessageHandler _mockHttp = new();

    public readonly string MockBaseUrl = "http://mock.url/*";

    public DateTime MockDate { get; } = new DateTime(2024, 01, 18);

    public HttpClient MockHttpClient => _mockHttp.ToHttpClient();

    public IOptions<ApiSettings> MockSettings => Options.Create(
        new ApiSettings()
        {
            LanguageCode = "EN",
            ExchangeRatesBaseUrl = MockBaseUrl,
            DailyRatesEndpoint = "daily"
        });
    public IEnumerable<Currency> CurrenciesInTestData { get; } = new List<Currency>()
    {
        new ("AUD"),
        new ("CAD"),
        new ("CNY"),
        new ("BGN"),
        new ("GBP"),
        new ("USD")
    };

    public IEnumerable<Currency> CurrenciesNotInTestData { get; } = new List<Currency>()
    {
        new ("XXX"),
        new ("YYY"),
        new ("ZZZ"),
    };

    public ExchangeRateData()
    {
        _apiJsonResponse = File.ReadAllText("Fixtures/DailyRateResponse.json");
        _mockHttp.When(MockBaseUrl).Respond("application/json", _apiJsonResponse);
    }


    public void Dispose()
    {
    }
}