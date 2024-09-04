using System.Text.Json;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;

namespace ExchangeRateUpdater;

public class ExchangeRateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ExchangeRatesConfig _exchangeRatesConfig;
    private readonly ILogger<ExchangeRateProvider> _logger;

    public ExchangeRateProvider(HttpClient httpClient, IOptions<ExchangeRatesConfig> exchangeRatesConfig, ILogger<ExchangeRateProvider> logger)
    {
        _httpClient = httpClient;
        _exchangeRatesConfig = exchangeRatesConfig.Value;
        _logger = logger;
    }
    
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates()
    {
        if (_exchangeRatesConfig.Currencies is null)
        {
            throw new Exception("Exchange Rates Currencies is null");
        }
        
        DailyExchangeRateResponse? rates = await GetDailyExchangeRates();
        if (rates?.Rates is null)
        {
            return Enumerable.Empty<ExchangeRate>();
        }

        return rates.Rates
            .Where(r => !string.IsNullOrWhiteSpace(r.CurrencyCode))
            .Where(r => _exchangeRatesConfig.Currencies.Contains(new Currency(r.CurrencyCode!)))
            .Select(r =>
            {
                var source = new Currency(r.CurrencyCode!);
                
                // Target seems to always be CZK, because the data is from the Czech Bank's point of view.
                var target = new Currency("CZK");
                var rate = r.Rate / r.Amount;
                    
                return new ExchangeRate(source, target, rate);
            });
    }
    
    private async Task<DailyExchangeRateResponse?> GetDailyExchangeRates()
    {
        DailyExchangeRateResponse? result = null;

        if (_exchangeRatesConfig.Url is null)
        {
            throw new Exception("Exchange Rates URL is null");
        }

        try
        {
            ResiliencePipeline resiliencePipeline = new ResiliencePipelineBuilder()
                .AddRetry(new RetryStrategyOptions
                {
                    ShouldHandle = new PredicateBuilder()
                        .Handle<HttpRequestException>()
                        .Handle<TaskCanceledException>(),
                    Delay = TimeSpan.FromSeconds(2),
                    MaxRetryAttempts = 2,
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,
                    OnRetry = onRetryArguments =>
                    {
                        _logger.LogError(onRetryArguments.Outcome.Exception,
                            "Error while retrieving daily exchange rates. Retry {retryNumber}",
                            onRetryArguments.AttemptNumber);
                        return ValueTask.CompletedTask;
                    }
                })
                .AddTimeout(TimeSpan.FromSeconds(30))
                .Build();

            using var response = await resiliencePipeline.ExecuteAsync(async cancellationToken =>
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(_exchangeRatesConfig.Url)
                };

                return await _httpClient.SendAsync(request, cancellationToken);
            });

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<DailyExchangeRateResponse>(responseBody,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
            else
            {
                _logger.LogWarning("Failed to retrieve daily exchange rates: {responseStatus}", response.StatusCode);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Can't retrieve daily exchange rates");
        }

        return result;
    }
}