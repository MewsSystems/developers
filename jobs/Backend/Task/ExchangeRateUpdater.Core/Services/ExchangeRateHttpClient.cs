using System.Net.Http.Json;
using ExchangeRateUpdater.Core.Exceptions;
using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Services;

public class ExchangeRateHttpClient : IExchangeRateHttpClient
{
    private readonly IApiConfiguration _apiConfiguration;

    public ExchangeRateHttpClient(IApiConfiguration apiConfiguration)
    {
        _apiConfiguration = apiConfiguration;
    }
    
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates()
    {
        using var httpClient = new HttpClient();

        try
        {
            var exchangeRates = await httpClient.GetFromJsonAsync<IEnumerable<ExchangeRate>>(_apiConfiguration.ApiUrl);
            return exchangeRates;
        }
        catch (Exception e)
        {
            throw new ExchangeRateException(e.Message, e);
        }
    }
}