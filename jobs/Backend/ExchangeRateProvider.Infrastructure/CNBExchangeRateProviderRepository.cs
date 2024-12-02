namespace ExchangeRateProvider.Infrastructure;

public class CNBExchangeRateProviderRepository : IExchangeRateProviderRepository
{
    private readonly ILogger<CNBExchangeRateProviderRepository> _logger;
    private readonly IApiHttpClient _httpClient;

    public CNBExchangeRateProviderRepository( ILogger<CNBExchangeRateProviderRepository> logger, IApiHttpClient httpClient )
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<CNBApiExchangeRateRecord>> GetDailyExchangeRatesAsync()
    {
        var result = new List<CNBApiExchangeRateRecord>();
        try
        {
            result = ( await _httpClient.GetDailyExchangeRatesAsync() ).ToList();
            _logger.LogDebug( "Received rates for {curr} currencies from Client", result.Count() );
        }
        catch( Exception ex )
        {
            _logger.LogError( ex, "Error occurred while fetching the exchange rates: {message}", ex.Message );
        }
        return result;
    }
}