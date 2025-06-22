using ExchangeRateModel;
using ExchangeRateService.Cache;
using ExchangeRateService.Client;
using Microsoft.Extensions.Logging;

namespace ExchangeRateService.Provider;

public class CNBExchangeRateProvider : IExchangeRateProvider
{

    private readonly ILogger<CNBExchangeRateProvider> _logger;
    private readonly ICNBRefitClient _client;
    private readonly IExchangeRateCache _cache;

    private readonly Currency _targetCurrency = new ("CZK");
    
    public CNBExchangeRateProvider(ILogger<CNBExchangeRateProvider> logger, ICNBRefitClient client, IExchangeRateCache cache)
    {
        _logger = logger;
        _client = client;
        _cache = cache;
    }

    public async Task<ExchangeRate> GetExchangeRate(Currency sourceCurrency, DateTime date)
    {
        _logger.LogInformation($"Getting exchange rate for {sourceCurrency} and {date}");
        var cached = await _cache.TryGetExchangeRate(new ExchangeRate(sourceCurrency, _targetCurrency, date));

        if (cached != null)
        {
            _logger.LogDebug($"Returning a cached value {cached}");
            return cached;
        }
        
        var requestResponse = await _client.GetDailyRates(date);

        if (!requestResponse.IsSuccessful || requestResponse.Content != null)
        {
            _logger.LogError($"Error {requestResponse.StatusCode} while retrieving daily rates for {date}: {requestResponse.Error?.Message}.");
            throw new Exception("Bad request");
        }

        var res = requestResponse.Content!.Rates
            .FirstOrDefault(r => r.CurrencyCode == sourceCurrency.Code);

        if (res == null)
        {
            _logger.LogDebug($"Doesn't contain exchange rate for the currency {sourceCurrency}");
            throw new Exception("No exchange rate for the currency");
        }

        var result = new ExchangeRate(sourceCurrency, _targetCurrency, res.Rate, date);
        await _cache.AddExchangeRate(result);
        
        return result;

    }

    public async Task<IList<ExchangeRate>> GetExchangeRates(IList<Currency> currencies, DateTime date)
    {
        var result = new List<ExchangeRate>();

        foreach (var currency in currencies)
        {
            var exchangeRate = await _cache.TryGetExchangeRate(new ExchangeRate(currency, _targetCurrency, date));
            
            if(exchangeRate != null)
                result.Add(exchangeRate);
        }
        
        var requestResponse = await _client.GetDailyRates(date);

        if (!requestResponse.IsSuccessful || requestResponse.Content == null)
            throw new Exception("Bad request");
        
        var res = requestResponse.Content!.Rates
            .Where(r => currencies.Contains(new Currency(r.CurrencyCode)))
            .ToList()
            .ConvertAll(r => new ExchangeRate(new Currency(r.CurrencyCode), _targetCurrency, r.Rate, date));

        return res;
    }
    
}