using ExchangeRateError;
using ExchangeRateModel;
using ExchangeRateService.CNB.Client.Interfaces;
using ExchangeRateService.CNB.Client.Model;
using Microsoft.Extensions.Logging;
using Refit;

namespace ExchangeRateService.CNB.Client;

/// <summary>
/// Implements communication with CNB API
/// </summary>
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

            // set the valid for since the 1st of the month, as of the CNB documentation
            foreach (var rate in fxRatesDailyMonth.Result.Rates)
            {
                var obtainedDate = DateTime.Parse(rate.ValidFor);
                rate.ValidFor = new DateTime(obtainedDate.Year, obtainedDate.Month + 1, 1).ToString("yyyy-MM-dd");
            }

            allRates = exratesDaily.Result.Rates.Concat(fxRatesDailyMonth.Result.Rates).ToList();
            var exchangeRates = allRates
                .ConvertAll(r =>
                    new ExchangeRate(new Currency(r.CurrencyCode), TargetCurrency, r.Rate, DateTime.Parse(r.ValidFor)));

            _logger.LogInformation($"Got {exchangeRates.Count} exchange rates");

            return exchangeRates;
        }
        catch (FormatException ex)
        {
            _logger.LogWarning($"Couldn't parse exchange rates: {ex.Message}");
            throw new ExchangeRateException($"Couldn't parse exchange rates: {ex.Message}", ex);
        }
        catch (ApiException ex) // non 2xx response
        {
            _logger.LogInformation(ex, $"Error occured getting daily rates: {ex.ReasonPhrase}");
            throw new ExchangeRateException($"Error occured getting daily rates: {ex.ReasonPhrase}", ex);
        }
        catch (HttpRequestException ex) // network error
        {
            _logger.LogWarning($"Can't connect to the server {ex.HttpRequestError}");
            throw new ExchangeRateException($"Can't connect to the server {ex.HttpRequestError}", ex);
        }
        
    }
    
}