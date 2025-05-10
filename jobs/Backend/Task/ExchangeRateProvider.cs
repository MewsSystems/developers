using ExchangeRateUpdater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public class ExchangeRateProvider
{
    private const string ApiUrl = "https://api.cnb.cz/";
    private const string ExchangeEndpoint = "cnbapi/exrates/daily";

    private static readonly HttpClient _client = new()
    {
        BaseAddress = new Uri(ApiUrl),
        Timeout = TimeSpan.FromSeconds(30)
    };

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    /// <remarks>
    /// The base currency is CZK, and the API returns exchange rates against CZK.
    /// </remarks>
    public static async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        if (currencies == null || !currencies.Any())
        {
            return Enumerable.Empty<ExchangeRate>();
        }

        CnbApiResponse? response;
        try
        {
            response = await _client.GetFromJsonAsync<CnbApiResponse>(ExchangeEndpoint);
        }
        catch (Exception ex)
        {
            // Log or handle errors as needed.
            throw new InvalidOperationException("Error fetching exchange rates", ex);
        }

        if (response?.Rates == null || !response.Rates.Any())
        {
            return Enumerable.Empty<ExchangeRate>();
        }

        // Create a dictionary with common Currency and CnbExchangeRate values.
        var currencyRateMap = new Dictionary<string, (Currency Currency, CnbExchangeRate Rate)>();
        foreach (var currency in currencies)
        {
            var rate = response.Rates.FirstOrDefault(r => string.Equals(r.CurrencyCode, currency.Code, StringComparison.OrdinalIgnoreCase));
            if (rate is not null)
            {
                currencyRateMap[currency.Code.ToUpperInvariant()] = (currency, rate);
            }
        }

        if (currencies.Any(c => c.Code == "CZK") && !currencyRateMap.ContainsKey("CZK"))
        {
            // Add the base currency (CZK) with a rate of 1 to the dictionary,
            // since it is not included in the response because CZK is the base currency returning from the API.
            currencyRateMap["CZK"] = (new Currency("CZK"), new CnbExchangeRate
            {
                CurrencyCode = "CZK",
                Amount = 1,
                Rate = 1
            });
        }

        if (currencyRateMap.Count < 2)
        {
            return Enumerable.Empty<ExchangeRate>();
        }

        var exchangeRates = new List<ExchangeRate>();
        foreach (var sourceEntry in currencyRateMap)
        {
            var (sourceCurrency, sourceRate) = sourceEntry.Value;
            
            foreach (var targetEntry in currencyRateMap)
            {
                if (sourceEntry.Key == targetEntry.Key)
                {
                    continue; // Skip the same currency
                }
                
                var (targetCurrency, targetRate) = targetEntry.Value;

                // Calculate the exchange rate between the two currencies.
                var sourceNormalized = sourceRate.Rate / sourceRate.Amount;
                var targetNormalized = targetRate.Rate / targetRate.Amount;
                var exchangeRateValue = 
                    Math.Round(sourceNormalized / targetNormalized, 3, MidpointRounding.AwayFromZero);
                exchangeRates.Add(new ExchangeRate(sourceCurrency, targetCurrency, exchangeRateValue));
            }
        }

        return exchangeRates;
    }
}
