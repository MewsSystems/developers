using Domain.Abstractions;
using Domain.Abstractions.Data;
using Domain.Configurations;
using Domain.Models;
using Infrastructure.Extensions;
using Microsoft.Extensions.Options;
using System.Web;

namespace Infrastructure.Services;

public class ExchangeRateProvider : IExchangeRateProvider
{
    protected readonly ICacheService _cacheService;
    protected readonly IHttpClientService _httpClientService;
    protected readonly CNBConfig _config;

    public ExchangeRateProvider(
        ICacheService cacheService,
        IHttpClientService httpClientService,
        IOptions<CNBConfig> config)
    {
        _cacheService = cacheService;
        _httpClientService = httpClientService;
        _config = config.Value;
    }

    const string QUERYDATE = "date";
    const string QUERYLANGUAGE = "lang";

    const string EXCHANGEDAILYRATESCACHEKEY = "ExchangeRates:Daily";

    public async Task<List<ExchangeRate>> GetDailyExchangeRates(Currency sourceCurrency, string language = "EN")
    {
        var result = _cacheService.Get<List<ExchangeRate>>(EXCHANGEDAILYRATESCACHEKEY);

        if (result is not null) 
        {
            return result;
        }

        return await GetExchangeDataAsync(sourceCurrency, language);

    }

    public async Task<ExchangeRate?> GetExchangeRate(Currency source, Currency target, string language = "EN")
    {
        var exchangeRates = await GetDailyExchangeRates(source, language);
        return exchangeRates
                .Where(x => x.SourceCurrency.Code == source.Code && x.TargetCurrency.Code == target.Code)
                .FirstOrDefault();
    }

    private async Task<List<ExchangeRate>> GetExchangeDataAsync(Currency sourceCurrency, string language = "EN")
    {
        var url = BuildURL(language);

        var rawData = await _httpClientService.GetJsonAsync<RawExchangeRates>(url);

        var convertedData = rawData.ConvertToExchangeRates(sourceCurrency);
        var tomorrowAt230pm =
            DateTime.Today
                .AddDays(1)
                .AddHours(_config.RefreshTimeHour)
                .AddMinutes(_config.RefreshTimeMinute);
        var expiryTime = tomorrowAt230pm - DateTime.UtcNow;

        _cacheService.Set(EXCHANGEDAILYRATESCACHEKEY, convertedData, expiryTime);

        return convertedData;
    }

    private string BuildURL(string language)
    {
        var shortDateInISOFormat = DateTime.Now.ToString("yyyy-MM-dd");

        var baseUri = new Uri($"{_config.BaseURL}{_config.ExchangeRateURL}");
        var uriBuilder = new UriBuilder(baseUri)
        {
            Port = -1
        };

        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query[QUERYDATE] = shortDateInISOFormat;
        query[QUERYLANGUAGE] = language;
        uriBuilder.Query = query.ToString();
        return uriBuilder.ToString();
    }
}
