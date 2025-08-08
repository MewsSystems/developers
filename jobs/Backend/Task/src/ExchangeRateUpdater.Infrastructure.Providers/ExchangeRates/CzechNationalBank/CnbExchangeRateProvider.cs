using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Providers;
using ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank;

public class CnbExchangeRateProvider(
    ICzechNationalBankApiClient cnbApiClient,
    ILogger<CnbExchangeRateProvider> logger)
    : IExchangeRateProvider
{
    public string Name => "CzechNationalBank";
    public string DefaultLanguage => "EN";
    public string DefaultCurrency => "CZK";
    public TimeZoneInfo TimeZone => TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague");

    public async Task<ExchangeRate[]> FetchAllCurrentAsync()
    {
        logger.LogInformation("Fetching current exchange rates from {ProviderName}", Name);
        
        try
        {
            var currentDate = DateTime.UtcNow;
            var pragueCurrentDate = TimeZoneInfo.ConvertTimeFromUtc(currentDate, TimeZone).Date;
            
            var result = await FetchByDateAsync(pragueCurrentDate);
            
            logger.LogInformation("Successfully fetched {Count} current exchange rates from {ProviderName}", 
                result.Length, Name);
            
            return result;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to fetch current exchange rates from {ProviderName}", Name);
            return [];
        }
    }

    public async Task<ExchangeRate[]> FetchByDateAsync(DateTime date)
    {
        logger.LogInformation("Fetching exchange rates from {ProviderName} for date {Date}", Name, date.ToString("yyyy-MM-dd"));
        
        try
        {
            // If the date is in the future, use the current date
            if (date > DateTime.UtcNow)
            {
                logger.LogWarning("Requested date {RequestedDate} is in the future, using current date", date.ToString("yyyy-MM-dd"));
                date = DateTime.UtcNow;
            }
            
            date = TimeZoneInfo.ConvertTimeFromUtc(date, TimeZone);
            
            logger.LogDebug("Fetching rates for {ProviderName} on {Date}", Name, date.ToString("yyyy-MM-dd"));

            var tasks = new List<Task<CnbExchangeRateResponse>>
            {
                FetchDailyRatesByDateAsync(date),
                FetchMonthlyRatesByDateAsync(date)
            };

            var responses = await Task.WhenAll(tasks);
            var result = FlattenAndConvertRatesToExchangeRates(responses);
            
            logger.LogInformation("Successfully fetched {Count} exchange rates from {ProviderName} for date {Date}", 
                result.Length, Name, date.ToString("yyyy-MM-dd"));
            
            return result;
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "HTTP request failed while fetching exchange rates from {ProviderName} for date {Date}. Status: {StatusCode}", Name, date.ToString("yyyy-MM-dd"), ex.StatusCode);
            return [];
        }
        catch (TaskCanceledException ex)
        {
            logger.LogWarning(ex, "Request timeout while fetching exchange rates from {ProviderName} for date {Date}", Name, date.ToString("yyyy-MM-dd"));
            return [];
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Unexpected error while fetching exchange rates from {ProviderName} for date {Date}", Name, date.ToString("yyyy-MM-dd"));
            return [];
        }
    }

    private async Task<CnbExchangeRateResponse> FetchDailyRatesByDateAsync(DateTime date)
    {
        logger.LogInformation("Fetching daily rates from API for {ProviderName} on {Date}", Name, date.ToString("yyyy-MM-dd"));
        
        try
        {
            var response = await cnbApiClient.GetFrequentExchangeRatesAsync(date.ToString("yyyy-MM-dd"));
            logger.LogInformation("Successfully fetched daily rates from API for {ProviderName}. Rate count: {Count}", Name, response.Rates?.Length ?? 0);
            return response;
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "HTTP request failed while fetching daily rates from API for {ProviderName} on {Date}. Status: {StatusCode}", Name, date.ToString("yyyy-MM-dd"), ex.StatusCode);
            throw;
        }
        catch (TaskCanceledException ex)
        {
            logger.LogWarning(ex, "Request timeout while fetching daily rates from API for {ProviderName} on {Date}", Name, date.ToString("yyyy-MM-dd"));
            throw;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Unexpected error while fetching daily rates from API for {ProviderName} on {Date}", Name, date.ToString("yyyy-MM-dd"));
            throw;
        }
    }

    private async Task<CnbExchangeRateResponse> FetchMonthlyRatesByDateAsync(DateTime date)
    {
        logger.LogInformation("Fetching monthly rates from API for {ProviderName} on {Date}", Name, date.ToString("yyyy-MM"));
        
        try
        {
            var response = await cnbApiClient.GetOtherExchangeRatesAsync(date.ToString("yyyy-MM"));
            logger.LogInformation("Successfully fetched monthly rates from API for {ProviderName}. Rate count: {Count}", Name, response.Rates?.Length ?? 0);
            return response;
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "HTTP request failed while fetching monthly rates from API for {ProviderName} on {Date}. Status: {StatusCode}", Name, date.ToString("yyyy-MM"), ex.StatusCode);
            throw;
        }
        catch (TaskCanceledException ex)
        {
            logger.LogWarning(ex, "Request timeout while fetching monthly rates from API for {ProviderName} on {Date}", Name, date.ToString("yyyy-MM"));
            throw;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Unexpected error while fetching monthly rates from API for {ProviderName} on {Date}", Name, date.ToString("yyyy-MM"));
            throw;
        }
    }

    private static ExchangeRate[] FlattenAndConvertRatesToExchangeRates(CnbExchangeRateResponse[] responses)
    {
        return responses.SelectMany(fxModel => fxModel.Rates)
            .Select(rateModel => rateModel.ToExchangeRate()).ToArray();
    }
}
    