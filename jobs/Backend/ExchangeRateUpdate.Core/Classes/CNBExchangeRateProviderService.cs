using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdate.Core.Classes;

public class CNBExchangeRateProviderService : IExchangeRateProviderService
{
    private readonly ILogger<CNBExchangeRateProviderService> _logger;
    private readonly IExchangeRateProviderRepository _exchangeRateProviderRepository;
    private readonly Currency _targetCurrency;
    //private Dictionary<string, ExchangeRateCacheItem> _cache = new Dictionary<string, ExchangeRateCacheItem>();

    public CNBExchangeRateProviderService( ILogger<CNBExchangeRateProviderService> logger,
                                          IExchangeRateProviderRepository exchangeRateProviderRepository,
                                          IConfiguration config )
    {
        _logger = logger;
        _exchangeRateProviderRepository = exchangeRateProviderRepository;
        _targetCurrency = new Currency( config.GetSection( "TargetCurrency" )?.Value );
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync( IEnumerable<Currency> currencies, bool useCache = false )
    {
        var result = new List<ExchangeRate>();
        try
        {
            //TODO
            //if ( useCache )
            //{
            //}
            var response = await _exchangeRateProviderRepository.GetDailyExchangeRatesAsync();
            _logger.LogDebug( "Received rates for {curr} currencies", response.Count() );
            var currenciesToLookup = currencies.Select( c => c.Code ).Distinct().ToList();
            var exchangeRatesReceived = response.ToDictionary( c => c.CurrencyCode, c => c.Rate / c.Amount );

            result = currenciesToLookup.Where( c => exchangeRatesReceived.ContainsKey( c ) )
                .Select( c => new ExchangeRate( new Currency( c ), _targetCurrency, exchangeRatesReceived[ c ].Value ) ).ToList();
        }
        catch( Exception ex )
        {
            _logger.LogError( ex, "Error occurred whilst getting exchange rates from CNB: {message}", ex.Message );
        }
        return result;
    }
}