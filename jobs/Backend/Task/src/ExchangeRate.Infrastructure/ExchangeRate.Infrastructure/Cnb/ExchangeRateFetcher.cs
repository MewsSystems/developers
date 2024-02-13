using System.Text.Json;
using ExchangeRate.Infrastructure.Cnb.Models;
using Microsoft.Extensions.Logging;
using Polly;

namespace ExchangeRate.Infrastructure.Cnb;

public interface IExchangeRateFetcher
{
    Task<IEnumerable<Models.ExchangeRate>> GetDailyExchangeRates(string date);
}

public class ExchangeRateFetcher(IHttpClientFactory  httpClientFactory, ILogger<ExchangeRateFetcher> logger) : IExchangeRateFetcher
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    private readonly ILogger<ExchangeRateFetcher> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    
    private const string BaseApiRoute = "https://api.cnb.cz/cnbapi";
    
    public async Task<IEnumerable<Models.ExchangeRate>> GetDailyExchangeRates(string date)
    {
        var url = $"{BaseApiRoute}/exrates/daily?date={date}&lang=EN";
        
        var responseBody = await FetchExchangeRates(url);
        var exchangeRates = Deserialize(responseBody);
        
        return exchangeRates.Rates;
    }

    private ExchangeRates Deserialize(string responseBody)
    {
        try
        {
            var exchangeRatesDeserialized = JsonSerializer.Deserialize<ExchangeRates>(responseBody) ??
                                            throw new InvalidOperationException("Deserialization of exchange rates returned null.");

            return exchangeRatesDeserialized;
        }
        catch (JsonException e)
        {
            _logger.LogError(e, "JSON parsing error. Could not deserialize exchange rates. The response body value: '{ResponseBody}'", responseBody);
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error. Could not deserialize exchange rates. The response body value: '{ResponseBody}'", responseBody);
            throw;
        }
    }

    private async Task<string> FetchExchangeRates(string url)
    {
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (exception, timeSpan, retryCount, _) =>
                {
                    _logger.LogError("Fetching data from {Url} failed. Retry {RetryCount} due to {ExceptionName}. Waiting {WaitDuration} seconds before next retry.",url, retryCount, exception.GetType().Name, timeSpan);
                });

        try
        {
            var response = await retryPolicy.ExecuteAsync(() => _httpClientFactory.CreateClient().GetAsync(url));
        
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e, "Could not retrieve exchange rates from {Url}.", url);
            throw;
        }
        catch (TaskCanceledException e)
        {
            _logger.LogError(e, "The request to {Url} timed out.", url);
            throw;
        }   
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected exception occurred when retrieving exchange rates from {Url}.", url);
            throw;
        }
    }
}