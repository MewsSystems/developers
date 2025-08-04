using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Providers;
using ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank.Models;

namespace ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank;

public class CnbExchangeRateProvider(ICzechNationalBankApiClient cnbApiClient, string providerName) : IExchangeRateProvider
{
    public string Name => providerName;
    public string DefaultLanguage => "EN";
    public string DefaultCurrency => "CZK";

    public async Task<ExchangeRate[]> FetchAllAsync()
    {
        // Todo Andrei: Pwedeng may rule dito pag di pa updated yung exchange rates
        var responses = await Task.WhenAll(cnbApiClient.GetFrequentExchangeRatesAsync(), cnbApiClient.GetOtherExchangeRatesAsync());
        if (responses[1].Rates.Length == 0)
            responses[1] =
                await cnbApiClient.GetOtherExchangeRatesAsync(DateTime.UtcNow.AddMonths(-1).ToString("yyyy-MM"));
        return ConvertRatesToExchangeRates(responses);
    }

    // Todo Andrei: How do we validate parameters?
    public async Task<ExchangeRate[]> FetchByDateAsync(DateTime date)
    {
        var responses = await Task.WhenAll(cnbApiClient.GetFrequentExchangeRatesAsync(date.ToString("yyyy-MM-dd")), cnbApiClient.GetOtherExchangeRatesAsync(date.ToString("yyyy-MM-dd")));
        return ConvertRatesToExchangeRates(responses);
    }

    private static ExchangeRate[] ConvertRatesToExchangeRates(CnbExchangeRateResponse[] responses)
    {
        return responses.SelectMany(fxModel => fxModel.Rates)
            .Select(rateModel => rateModel.ToExchangeRate()).ToArray();
    }
}
