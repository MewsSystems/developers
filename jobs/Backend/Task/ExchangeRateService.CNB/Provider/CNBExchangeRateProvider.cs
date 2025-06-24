using ExchangeRateModel;
using ExchangeRateService.Cache;
using ExchangeRateService.CNB.Client.Interfaces;
using ExchangeRateService.Provider;
using Microsoft.Extensions.Logging;
using Refit;

namespace ExchangeRateService.CNB.Provider;

public class CNBExchangeRateProvider : IExchangeRateProvider
{

    private readonly ILogger<CNBExchangeRateProvider> _logger;
    private readonly ICNBClient _client;
    private readonly IExchangeRateCache _cache;

    public CNBExchangeRateProvider(ILogger<CNBExchangeRateProvider> logger, ICNBClient client, IExchangeRateCache cache)
    {
        _logger = logger;
        _client = client;
        _cache = cache;
    }

    public async Task<ExchangeRate> GetExchangeRate(Currency sourceCurrency, DateTime date)
    {
        _logger.LogInformation($"Getting exchange rate for {sourceCurrency} and {date}");
        var cached = await _cache.TryGetExchangeRate(new ExchangeRate(sourceCurrency, _client.TargetCurrency, date));

        if (cached != null)
        {
            _logger.LogDebug($"Returning a cached value {cached}");
            return cached;
        }

        var exchangeRates = await _client.GetExchangeRates([sourceCurrency], date);

        await _cache.AddExchangeRates(exchangeRates);
        
        var res = exchangeRates
            .FirstOrDefault(r => r.SourceCurrency.Code == sourceCurrency.Code);

        if (res == null)
        {
            _logger.LogDebug($"Doesn't contain exchange rate for the currency {sourceCurrency}");
            throw new Exception("No exchange rate for the currency");
        }
        
        return res;
    }

    public async Task<IList<ExchangeRate>> GetExchangeRates(IList<Currency> currencies, DateTime date)
    {
        _logger.LogInformation($"Getting exchange rates for {currencies.Count} currencies and {date:yyyy-MM-dd}");
        var wantedRates = currencies
            .ToList().ConvertAll(c => new ExchangeRate(c, _client.TargetCurrency, date));
        var resultRates = await _cache.TryGetExchangeRates(wantedRates);
        
        if (resultRates.Count == currencies.Count)
        {
            _logger.LogInformation("Returning cached exchange rates");
            return resultRates;
        }
        
        var exchangeRates = await _client.GetExchangeRates(currencies, date);
        
        await _cache.AddExchangeRates(exchangeRates);
        
        var obtainedRates = exchangeRates
            .Where(r => resultRates.All(cached => cached.ExchangeRateName() != r.ExchangeRateName()) &&
                        currencies.Contains(r.SourceCurrency)).ToList();

        var res = resultRates.Concat(obtainedRates).ToList();

        _logger.LogInformation($"Returning {res.Count} exchange rates");
        
        return res;
    }
    
}