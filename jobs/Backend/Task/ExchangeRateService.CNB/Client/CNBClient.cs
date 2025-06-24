using ExchangeRateModel;
using ExchangeRateService.CNB.Client.Interfaces;
using ExchangeRateService.CNB.Client.Model;
using Microsoft.Extensions.Logging;
using Refit;

namespace ExchangeRateService.CNB.Client;

public class CNBClient : ICNBClient
{
    
    private readonly ILogger<CNBClient> _logger;
    private readonly ICNBRefitClient _client;
    public Currency TargetCurrency { get; } = new ("CZK");

    public CNBClient(ILogger<CNBClient> logger)
    {
        _logger = logger;
        _client = RestService.For<ICNBRefitClient>("https://api.cnb.cz");
    }
    
    public async Task<IList<ExchangeRate>> GetExchangeRates(IList<Currency> currencies, DateTime date)
    {
        _logger.LogInformation("Getting exchange rates");
        
        var exratesDaily = _client.GetExratesDailyRates(date);
        // -1 month, because for the specified month,
        // the fx rate is set at the end of the previous month
        var fxRatesDailyMonth = _client.GetFXRatesDailyMonthRates(date.AddMonths(-1));

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
            .ConvertAll(r => new ExchangeRate(new Currency(r.CurrencyCode), TargetCurrency, r.Rate, date));
        
        _logger.LogInformation($"Got {exchangeRates.Count} exchange rates");
        
        return exchangeRates;
    }
    
}