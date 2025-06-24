using ExchangeRateError;
using ExchangeRateModel;
using ExchangeRateService.Cache;
using ExchangeRateService.CNB.Client.Interfaces;
using ExchangeRateService.Provider;
using Microsoft.Extensions.Logging;
using Refit;

namespace ExchangeRateService.CNB.Provider;

/// <summary>
/// Implements provider for the CNB
/// </summary>
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

        if (await _cache.TryGetExchangeRate(new ExchangeRate(sourceCurrency, _client.TargetCurrency, date), out var cached))
        {
            _logger.LogDebug($"Returning a cached value {cached}");
            return cached;
        }

        var exchangeRates = await _client.GetExchangeRates([sourceCurrency], date);

        await CacheRates(exchangeRates, date);
        
        var res = exchangeRates
            .FirstOrDefault(r => r.SourceCurrency.Code == sourceCurrency.Code);

        if (res == null)
        {
            _logger.LogDebug($"Doesn't contain exchange rate for the currency {sourceCurrency}");
            throw new ExchangeRateException("No exchange rate for the currency");
        }
        
        return new ExchangeRate(res.SourceCurrency, res.TargetCurrency, res.Value, date);
    }

    public async Task<IList<ExchangeRate>> GetExchangeRates(IList<Currency> currencies, DateTime date)
    {
        _logger.LogInformation($"Getting exchange rates for {currencies.Count} currencies and {date:yyyy-MM-dd}");
        var wantedRates = currencies
            .ToList().ConvertAll(c => new ExchangeRate(c, _client.TargetCurrency, date));
        var resultRates = await _cache.GetExchangeRates(wantedRates);
        
        if (resultRates.Count == currencies.Count)
        {
            _logger.LogInformation("Returning cached exchange rates");
            return resultRates;
        }
        
        var exchangeRates = await _client.GetExchangeRates(currencies, date);
        
        await CacheRates(exchangeRates, date);
        
        var obtainedRates = exchangeRates
            .Where(r => resultRates.All(cached => cached.ExchangeRateName() != r.ExchangeRateName()) &&
                        currencies.Contains(r.SourceCurrency)).ToList();

        var res = resultRates.Concat(obtainedRates).ToList();

        _logger.LogInformation($"Returning {res.Count} exchange rates");
        
        return res;
    }

    private async Task CacheRates(IList<ExchangeRate> exchangeRates, DateTime date)
    {

        var multipleRates = new List<ExchangeRate>();

        foreach (var rate in exchangeRates)
        {
            var tmpDate = new DateTime(rate.Date.Year, rate.Date.Month, rate.Date.Day);
            while (tmpDate <= date)
            {
                multipleRates.Add(new ExchangeRate(rate.SourceCurrency, rate.TargetCurrency, rate.Value, tmpDate));
                tmpDate = tmpDate.AddDays(1);
            }
            
        }
        
        await _cache.AddExchangeRates(multipleRates);
    }
    
}