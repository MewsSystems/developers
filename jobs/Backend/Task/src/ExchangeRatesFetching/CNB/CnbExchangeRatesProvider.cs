using ExchangeRatesFetching.CNB.Response;
using ExchangeRatesUpdater.Common;
using ExchangeRateUpdater;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using System.Text.Json;

namespace ExchangeRatesFetching.CNB;

internal class CnbExchangeRatesProvider : IExchangeRatesProvider
{
    private const string TargetCurrencyCode = "CZK";

    private readonly ILogger<CnbExchangeRatesProvider> logger;
    private readonly AppConfiguration config;
    private readonly HttpClient httpClient;

    public string BankName => Constants.CnbName;

    public CnbExchangeRatesProvider(IOptions<AppConfiguration> config, ILogger<CnbExchangeRatesProvider> logger, IHttpClientFactory clientFactory)
    {
        this.config = config.Value;
        this.logger = logger;
        httpClient = clientFactory.CreateClient();
    }

    public async Task<IEnumerable<ExchangeRate>> GetRatesForCurrenciesAsync(IEnumerable<string> currencies)
    {
        config.ProviderAPIs.TryGetValue(BankName, out var apiUrl);
        if (string.IsNullOrEmpty(apiUrl)) {
            logger.LogError("Configuration file does not contain CNB API URL");
            return Enumerable.Empty<ExchangeRate>();
        }

        string contentString = await FetchDataFromApiAsync(apiUrl);

        ExRateDailyResponse? cnbResponse = DeserializeResponse(contentString);
        if (cnbResponse == null) {
            logger.LogError("Could not deserialize response from CNB API");
            return Enumerable.Empty<ExchangeRate>();
        }

        IEnumerable<ExchangeRate> rates = ConvertToExchangeRates(cnbResponse, currencies);

        return rates;
    }

    private async Task<string> FetchDataFromApiAsync(string apiUrl)
    {
        const int RetryCount = 3;
        const int TimeoutSeconds = 10;
        const int RetrySeconds = 10;

        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(RetryCount, retryAttempt => TimeSpan.FromSeconds(RetrySeconds));

        var timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromSeconds(TimeoutSeconds));

        HttpResponseMessage response = await retryPolicy
            .WrapAsync(timeoutPolicy)
            .ExecuteAsync(() => httpClient.GetAsync(apiUrl));

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    private static ExRateDailyResponse? DeserializeResponse(string contentString)
    {
        JsonSerializerOptions options = new() {
            PropertyNameCaseInsensitive = true
        };
        return JsonSerializer.Deserialize<ExRateDailyResponse>(contentString, options);
    }

    private static IEnumerable<ExchangeRate> ConvertToExchangeRates(ExRateDailyResponse cnbResponse, IEnumerable<string> currencies)
    {
        HashSet<string> currencySet = new(currencies, StringComparer.OrdinalIgnoreCase);

        IEnumerable<ExRateDailyRest> filteredRates = cnbResponse.Rates
            .Where(rate => currencySet.Contains(rate.CurrencyCode));

        IEnumerable<ExchangeRate> exchangeRates = filteredRates.Select(rate => new ExchangeRate(
            new Currency(rate.CurrencyCode),
            new Currency(TargetCurrencyCode),
            rate.Rate / rate.Amount));

        return exchangeRates;
    }
}
