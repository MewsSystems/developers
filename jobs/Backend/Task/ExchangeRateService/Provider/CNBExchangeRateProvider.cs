using ExchangeRateModel;
using ExchangeRateService.Cache;
using ExchangeRateService.Client;
using ExchangeRateService.Client.Model.CNB;
using Microsoft.Extensions.Logging;
using Refit;

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

        var exchangeRate = await GetExchangeRatesData([sourceCurrency], date);

        var res = exchangeRate
            .FirstOrDefault(r => r.SourceCurrency.Code == sourceCurrency.Code);

        if (res == null)
        {
            _logger.LogDebug($"Doesn't contain exchange rate for the currency {sourceCurrency}");
            throw new Exception("No exchange rate for the currency");
        }

        var result = new ExchangeRate(sourceCurrency, _targetCurrency, res.Value, date);
        
        return result;
    }

    public async Task<IList<ExchangeRate>> GetExchangeRates(IList<Currency> currencies, DateTime date)
    {
        _logger.LogInformation($"Getting exchange rates for {currencies.Count} currencies and {date:yyyy-MM-dd}");
        var wantedRates = currencies
            .ToList().ConvertAll(c => new ExchangeRate(c, _targetCurrency, date));
        var resultRates = await _cache.TryGetExchangeRates(wantedRates);

        if (resultRates.Count == currencies.Count)
        {
            _logger.LogInformation("Returning cached exchange rates");
            return resultRates;
        }
        
        var exchangeRates = await GetExchangeRatesData(currencies, date);
        
        var obtainedRates = exchangeRates
            .Where(r => resultRates.All(cached => cached.ExchangeRateName() != r.ExchangeRateName()) &&
                        currencies.Contains(r.SourceCurrency)).ToList();

        var res = resultRates.Concat(obtainedRates).ToList();

        _logger.LogInformation($"Returning {res.Count} exchange rates");
        
        return res;
    }

    private async Task<IList<ExchangeRate>> GetExchangeRatesData(IList<Currency> currencies, DateTime date)
    {
        
        var exratesDaily = _client.GetExratesDailyRates(date);
        var fxRatesDailyMonth = _client.GetFXRatesDailyMonthRates(date);

        var allRates = new List<ExchangeRateBody>();
        
        try
        {
            await Task.WhenAll(exratesDaily, fxRatesDailyMonth);
            
            allRates = exratesDaily.Result.Rates.Concat(fxRatesDailyMonth.Result.Rates).ToList();
        }
        catch (ApiException ex) // non 2xx response
        {
            _logger.LogInformation(ex, $"Error occured getting daily rates: {ex.ReasonPhrase}");
            throw new Exception($"Error occured getting daily rates: {ex.ReasonPhrase}", ex);
        }
        catch (HttpRequestException ex) // network error
        {
            _logger.LogWarning($"Can't connect to the server {ex.HttpRequestError}");
            throw new Exception($"Can't connect to the server {ex.HttpRequestError}", ex);
        }
        
        var exchangeRates = allRates
            .ConvertAll(r => new ExchangeRate(new Currency(r.CurrencyCode), _targetCurrency, r.Rate, date));
        
        await _cache.AddExchangeRates(exchangeRates);
        
        return exchangeRates;
    }
    
}