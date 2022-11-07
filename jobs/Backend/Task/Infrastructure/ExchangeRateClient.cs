using System;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.App;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Infrastructure;

public class ExchangeRateClient : IExchangeRateClient
{
    private readonly HttpClient _client;
    private readonly ILogger<ExchangeRateClient> _logger;

    public ExchangeRateClient(HttpClient client, ILogger<ExchangeRateClient> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<string> GetExchangeRateAsync(DateTime date)
    {
        try
        {
            var response = await _client.GetAsync($"?date={date:dd.MM.yyyy}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e, $"Exchange rate server returns error: {e.StatusCode}");
            throw;
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "Error occured during sending request to the exchange rate server");
            throw;
        }
    }
}