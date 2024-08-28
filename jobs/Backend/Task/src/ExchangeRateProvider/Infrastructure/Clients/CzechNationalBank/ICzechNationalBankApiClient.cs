using Refit;

namespace ExchangeRateUpdater.Infrastructure.Clients.CzechNationalBank;

public interface ICzechNationalBankApiClient
{
    [Get("/exrates/daily")]
    Task<GetCzechNationalBankExchangeRatesResponse> GetExchangeRatesAsync(string date, string? lang = "EN");
}
