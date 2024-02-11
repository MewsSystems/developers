using System.Text.Json;
using Microsoft.Extensions.Logging;

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

    private Models.ExchangeRates Deserialize(string responseBody)
    {
        try
        {
            var exchangeRatesDeserialized = JsonSerializer.Deserialize<Models.ExchangeRates>(responseBody) ??
                throw new InvalidOperationException("Deserialization of exchange rates returned null.");

            return exchangeRatesDeserialized;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not deserialize exchange rates. The response body value: '{ResponseBody}'", responseBody);
            throw;
        }
    }

    private async Task<string> FetchExchangeRates(string url)
    {
        try
        {
            //Note: possibly use polly for retrying the request when the status code is 500
            var response = await _httpClientFactory.CreateClient().GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e, "Could not retrieve exchange rates.");
            throw;
        }
    }
}