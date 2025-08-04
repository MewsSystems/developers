using ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank.Models;
using Refit;

namespace ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank;

public interface ICzechNationalBankApiClient
{
    [Get("/exrates/daily")]
    Task<CnbExchangeRateResponse> GetFrequentExchangeRatesAsync(string? date = "", string? lang = "EN");
    
    [Get("/fxrates/daily-month")]
    Task<CnbExchangeRateResponse> GetOtherExchangeRatesAsync(string? yearMonth = "", string? lang = "EN");
}