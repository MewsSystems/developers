using System;
using System.Threading.Tasks;
using ExchangeRateUpdater.ExchangeRateSource;
using RestSharp;

namespace ExchangeRateUpdater.Client;

internal class ExchangeRateClient : IExchangeRateClient, IDisposable
{
    private readonly RestClient _client;
    private readonly IExchangeRateSource _exchangeRateSource;

    public ExchangeRateClient(IExchangeRateSource exchangeRateSource)
    {
        _exchangeRateSource = exchangeRateSource;
        _client = new RestClient(_exchangeRateSource.BaseRestUrl);
    }
    public void Dispose()
    {
        _client?.Dispose();
        GC.SuppressFinalize(this); // Prevents garbage collector calling object finalizer.
    }

    public async Task<string> GetExchangeRateAsync()
    {
        var request = new RestRequest(_exchangeRateSource.EndPoint);
        var response = await _client.ExecuteGetAsync(request);
        if (!response.IsSuccessful || response.Content == null)
            throw new ApplicationException($"Error fetching exchange rates: {response.ErrorMessage}");

        return response.Content;
    }
}