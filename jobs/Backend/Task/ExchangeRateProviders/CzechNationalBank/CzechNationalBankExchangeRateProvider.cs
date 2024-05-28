using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace ExchangeRateUpdater.ExchangeRateProviders.CzechNationalBank;

/*
 * Czech National Bank Exchange Rate Provider.
 * Provides CZK Exchange rates.
 *
 * See: https://api.cnb.cz/cnbapi/swagger-ui.html
 * For swagger documentation on API
 */
public class CzechNationalBankExchangeRateProvider : IExchangeRateProvider
{
    private readonly HttpClient _httpClient;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly CzechNationalBankExchangeRateProviderConfiguration _configuration;
    
    private readonly Currency CZK = new("Czech Republic", "koruna", "CZK");

    public CzechNationalBankExchangeRateProvider(HttpClient httpClient, IDateTimeProvider dateTimeProvider, IOptions<CzechNationalBankExchangeRateProviderConfiguration> configuration)
    {
        _httpClient = httpClient;
        _dateTimeProvider = dateTimeProvider;
        _configuration = configuration.Value;
    }

    public async Task<Result<IEnumerable<ExchangeRate>>> GetExchangeRates(IReadOnlyCollection<string> currencyCodes)
    {
        try
        {
            await using var jsonDataStream = await _httpClient.GetStreamAsync(BuildApiUri(_configuration.ApiUrl, _configuration.ApiDateFormat));

            var czkExchangeRates = JsonSerializer.Deserialize<CzechNationalBankExchangeRatesResponse>(jsonDataStream,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            if (czkExchangeRates is null)
            {
                return Result<IEnumerable<ExchangeRate>>.Failed("Failed to deserialize response from Czech National Bank");
            }

            return Result<IEnumerable<ExchangeRate>>.Succeeded(czkExchangeRates.Rates
                .Where(rate => currencyCodes.Contains(rate.CurrencyCode, StringComparer.OrdinalIgnoreCase))
                .Select(rate =>
                    new ExchangeRate(CZK, new Currency(rate.Country, rate.Currency, rate.CurrencyCode), rate.Rate)));
        }
        catch (HttpRequestException httpRequestException)
        {
            return Result<IEnumerable<ExchangeRate>>.Failed(httpRequestException.Message);
        }
    }

    private Uri BuildApiUri(string apiUrl, string dateFormat)
    {
        var apiUri = new Uri(apiUrl);

        var query = HttpUtility.ParseQueryString(apiUri.Query);
        query["date"] = _dateTimeProvider.Now.ToString(dateFormat);

        var uriBuilder = new UriBuilder(apiUri)
        {
            Query = query.ToString() ?? string.Empty
        };

        return uriBuilder.Uri;
    }
}