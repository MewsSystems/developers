using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExchangeRateUpdater;

public abstract class BankServiceBase
{
    private readonly HttpClient _client;

    protected BankServiceBase(HttpClient client)
    {
        _client = client;
    }
        
    protected async Task<TResp> Get<TResp>(string urlPath)
    {
        var response = await _client.GetAsync(urlPath);
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<TResp>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
    }
    
    //PUT
    
    //POST
}