using System.Collections.Specialized;
using Mews.ERP.AppService.Data.Models;
using Mews.ERP.AppService.Features.Fetch.Builders.Interfaces;
using Mews.ERP.AppService.Features.Fetch.Models;
using Mews.ERP.AppService.Features.Fetch.Networking.Providers.Base;
using Mews.ERP.AppService.Features.Fetch.Networking.Providers.Interfaces;
using Mews.ERP.AppService.Features.Fetch.Networking.Responses;
using Mews.ERP.AppService.Shared.Caching.Interfaces;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace Mews.ERP.AppService.Features.Fetch.Networking.Providers;

public class CnbExchangeRatesProvider : ExchangeRateProvider, ICnbExchangeRatesProvider
{
    private const string DailyExchangeRatesResource =
        $"{Constants.CzechNationalBank.CzechApiRoute}/{Constants.CzechNationalBank.ExchangeRatesDailyApiRoute}";

    private const string DailyExchangeRatesCacheKey = "CnbDailyExchangeRates";
    
    private readonly IRestRequestBuilder requestBuilder;

    private readonly ICachingWrapper cachingWrapper;
    
    public CnbExchangeRatesProvider(
        IRestRequestBuilder requestBuilder,
        ICachingWrapper cachingWrapper,
        IRestClient restClient, 
        ILogger<CnbExchangeRatesProvider> logger) : base(restClient, logger)
    {
        this.requestBuilder = requestBuilder;
        this.cachingWrapper = cachingWrapper;
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken = default)
    {
        if (!currencies.Any())
        {
            return Enumerable
                .Empty<ExchangeRate>();
        }

        var czechExchangeRates = await GetDailyRates();

        var filteredRates = czechExchangeRates!
            .Where(rate => currencies.Any(c => c.Code == rate.CurrencyCode))
            .Select(exchangeRate => new ExchangeRate
            (
                new Currency(exchangeRate.CurrencyCode),
                new Currency(Constants.CzechNationalBank.CzechCurrencyCode),
                exchangeRate.Amount,
                exchangeRate.Rate,
                exchangeRate.ValidFor,
                exchangeRate.Country
            ))
            .ToList();

        return filteredRates;
    }

    private async Task<IEnumerable<ExchangeRateResponse>?> GetDailyRates()
    {
        var today = DateTime.UtcNow;
        var parameters = new NameValueCollection
        {
            {"date", today.ToString("yyyy-MM-dd")},
            {"lang", Constants.CzechNationalBank.EnglishLanguageIso2Code}
        };
        
        var isResponseCached = cachingWrapper.TryGetValue(DailyExchangeRatesCacheKey, out IEnumerable<ExchangeRateResponse>? czechExchangeRates);

        if (!isResponseCached)
        {
            czechExchangeRates = await GetRates(requestBuilder.Build(DailyExchangeRatesResource, parameters));
            
            cachingWrapper.Set(DailyExchangeRatesCacheKey, czechExchangeRates, DateTimeOffset.Now.AddDays(1));
        }

        return czechExchangeRates;
    }
}