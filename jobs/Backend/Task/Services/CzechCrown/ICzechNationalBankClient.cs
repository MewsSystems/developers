using Services.CzechCrown.Models;

namespace Services.CzechCrown;

public interface ICzechNationalBankClient
{
    Task<CzkExchangeRateResponse> GetExchangeRates(DateOnly date, CancellationToken ct = default);
    Task<CzkExchangeRateResponse> GetOtherExchangeRates(DateOnly date, CancellationToken ct = default);
}