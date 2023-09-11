using System.Collections.Specialized;
using Mews.ERP.AppService.Data.Models;
using Mews.ERP.AppService.Features.Fetch.Builders.Interfaces;
using Mews.ERP.AppService.Features.Fetch.Models;
using Mews.ERP.AppService.Features.Fetch.Networking.Providers.Base;
using Mews.ERP.AppService.Features.Fetch.Networking.Providers.Interfaces;
using Mews.ERP.AppService.Features.Fetch.Networking.Responses;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace Mews.ERP.AppService.Features.Fetch.Networking.Providers;

public class CnbExchangeRatesProvider : ExchangeRateProvider, ICnbExchangeRatesProvider
{
    private const string DailyExchangeRatesResource =
        $"{Constants.CzechNationalBank.CzechApiRoute}/{Constants.CzechNationalBank.ExchangeRatesDailyApiRoute}";
    
    private readonly IRestRequestBuilder requestBuilder;
    
    public CnbExchangeRatesProvider(
        IRestRequestBuilder requestBuilder,
        IRestClient restClient, 
        ILogger<CnbExchangeRatesProvider> logger) : base(restClient, logger)
    {
        this.requestBuilder = requestBuilder;
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken = default)
    {
        if (!currencies.Any())
        {
            return Enumerable
                .Empty<ExchangeRate>();
        }

        var czechExchangeRates = await GetDailyRates();

        var filteredRates = czechExchangeRates
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

    private async Task<IEnumerable<ExchangeRateResponse>> GetDailyRates()
    {
        var today = DateTime.UtcNow;
        var query = new NameValueCollection
        {
            {"date", today.ToString("yyyy-MM-dd")},
            {"lang", Constants.CzechNationalBank.EnglishLanguageIso2Code}
        };
        
        return await GetRates(requestBuilder.Build(DailyExchangeRatesResource, query));
    }
}