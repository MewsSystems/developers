namespace ExchangeRateUpdater;

public class BOERateProviderService : IExchangeRateProvider
{
    readonly ILogger<BOERateProviderService> _logger;
    readonly AppConfig _appConfig;
    readonly HttpClient _httpClient;

    public BOERateProviderService(ILogger<BOERateProviderService> logger, IOptions<AppConfig> appConfig, HttpClient httpClient)
    {
        _logger = logger;
        _appConfig = appConfig.Value;
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates()
    {
        await Task.Delay(0);
        throw new NotImplementedException("Bank Of England exchange rate provider not yet added...");
    }
}