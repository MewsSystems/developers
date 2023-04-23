using System;
using System.Threading.Tasks;
using RestSharp;

namespace ExchangeRateUpdater.Client;

internal class ExchangeRateClient : IExchangeRateClient, IDisposable
{
    private readonly RestClient _client;

    public ExchangeRateClient()
    {
        _client = new RestClient("https://api.cnb.cz/cnbapi/");
    }
    public void Dispose()
    {
        _client?.Dispose();
        GC.SuppressFinalize(this); // Prevents garbage collector calling object finalizer.
    }

    public async Task<string> GetExchangeRateAsync()
    {
        var request = new RestRequest("exrates/daily");
        var response = await _client.ExecuteGetAsync(request);
        if (!response.IsSuccessful || response.Content == null)
            throw new ApplicationException($"Error fetching exchange rates: {response.ErrorMessage}");

        return response.Content;
    }
}