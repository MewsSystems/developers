using System.Net.Http.Json;
using ExchangeRateUpdater.Dto;

namespace ExchangeRateUpdater;

public class CzechExchangeRateFetcher : IExchangeRateFetcher
{
    public async Task<ExchangeRatesBo?> FetchCurrentAsync()
    {
        // Todo here could be some caching mechanism
        // Todo here could be some retry mechanism - TransientHttpPolicy
        using var client = new HttpClient();
        var content = (await client
                    // Todo it could be configurable
                .GetAsync( "https://api.cnb.cz/cnbapi/exrates/daily"))
            .EnsureSuccessStatusCode()
            .Content;    
            
        return await content.ReadFromJsonAsync<ExchangeRatesBo>();
    }
}