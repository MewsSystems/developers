using ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank.Models;
using Refit;

namespace ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank;

public interface ICzechNationalBankApiClient
{
    // Todo Andrei: Pag di naccall yung endpoint walang error messages
    [Get("/exrates/daily")]
    Task<CnbExchangeRateResponse> GetFrequentExchangeRatesAsync(string? date = null, string? lang = "EN");
    
    [Get("/fxrates/daily-month")]
    Task<CnbExchangeRateResponse> GetOtherExchangeRatesAsync(string? yearMonth = null, string? lang = "EN");
}