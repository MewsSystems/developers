using ExchangeRate.Core.Models.ClientResponses;

namespace ExchangeRate.Core.ExchangeRateSourceClients;

public class CnbExchangeRateClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CnbExchangeRateClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<CnbExchangeRateResponse>> GetExchangeRatesAsync(string urlPath)
    {
        throw new NotImplementedException();
    }
}
