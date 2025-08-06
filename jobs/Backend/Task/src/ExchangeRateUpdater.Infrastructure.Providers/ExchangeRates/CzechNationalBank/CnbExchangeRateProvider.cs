using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Providers;
using ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank;

public class CnbExchangeRateProvider(ICzechNationalBankApiClient cnbApiClient, IDistributedCache cache) : IExchangeRateProvider
{
    public string Name => "CzechNationalBank";
    public string DefaultLanguage => "EN";
    public string DefaultCurrency => "CZK";
    public TimeZoneInfo DefaultTimezone => TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague");

    public async Task<ExchangeRate[]> FetchAllAsync()
    {
        // Todo Andrei: Pwedeng may rule dito pag di pa updated yung exchange rates
        var responses = await Task.WhenAll(cnbApiClient.GetFrequentExchangeRatesAsync(), cnbApiClient.GetOtherExchangeRatesAsync());
        
        if (responses[1].Rates.Length == 0)
            responses[1] =
                await cnbApiClient.GetOtherExchangeRatesAsync(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, DefaultTimezone).ToLocalTime().AddMonths(-1).ToString("yyyy-MM"));
        
        return ConvertRatesToExchangeRates(responses);
    }

    // Todo Andrei: How do we validate parameters?
    public async Task<ExchangeRate[]> FetchByDateAsync(DateTime date)
    {
        date = TimeZoneInfo.ConvertTimeFromUtc(date, DefaultTimezone);
        var responses = await Task.WhenAll(cnbApiClient.GetFrequentExchangeRatesAsync(date.ToString("yyyy-MM-dd")), cnbApiClient.GetOtherExchangeRatesAsync(date.ToString("yyyy-MM")));
        return ConvertRatesToExchangeRates(responses);
    }

    private static ExchangeRate[] ConvertRatesToExchangeRates(CnbExchangeRateResponse[] responses)
    {
        return responses.SelectMany(fxModel => fxModel.Rates)
            .Select(rateModel => rateModel.ToExchangeRate()).ToArray();
    }
}
    