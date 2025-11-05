using System.Text.Json;
using ExchangeRateUpdater.Core.ApiVendors;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Infrastructure.Dtos;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Infrastructure.ExchangeRateVendors;

public class CurrencyApiExchangeRateVendor(
        HttpClient httpClient,
        ILogger<CurrencyApiExchangeRateVendor> logger
    ) : IExchangeRateVendor
{
    public async Task<List<ExchangeRate>> GetExchangeRates(string baseCurrencyCode)
    {
        try
        {
            logger.LogInformation("Retrieving exchange rates for base currency '{BaseCurrencyCode}'" , baseCurrencyCode);
            
            string requestUri = $"latest?base_currency={baseCurrencyCode}";
            var result = await httpClient.GetAsync(requestUri);

            if (!result.IsSuccessStatusCode)
            {
                var reasonPhrase = await result.Content.ReadAsStringAsync();
                logger.LogError("Could not retrieve exchange rates: '{StatusCode}', Reason: '{ReasonPhrase}'.", result.StatusCode, reasonPhrase);
                return [];
            }
            
            logger.LogInformation("Successfully retrieved exchange rates.");
            var jsonString = await result.Content.ReadAsStringAsync();
            
            var exchangeRates = JsonSerializer.Deserialize<CurrencyApiResponse>(jsonString);

            return exchangeRates == null ? [] : exchangeRates.Data.Select(rate => new ExchangeRate(new Currency(baseCurrencyCode), new Currency(rate.Key), rate.Value.Value)).ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Could not retrieve exchange rates: '{EMessage}'.", e.Message);
            return [];
        }
    }
}