using ExchangeRateService.Domain;

namespace ExchangeRateService.ExternalServices;

internal class CNBClientService : ICNBClientService
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    
    public CNBClientService(HttpClient httpClient, IMapper mapper)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));   
    }

    public async ValueTask<ExchangeRate[]> GetDailyExchangeRatesAsync(CancellationToken cancellationToken = default)
    {
        var exRates = await _httpClient.GetFromJsonAsync<CNBDailyExratesResponse>("/cnbapi/exrates/daily?lang=EN", cancellationToken);
        return _mapper.Map<ExchangeRate[]>(exRates!.Rates);
    }
}