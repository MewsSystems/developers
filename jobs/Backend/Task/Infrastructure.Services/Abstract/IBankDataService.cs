using Infrastructure.Models.CzechNationalBankModels;
using Infrastructure.Models.Responses;

namespace Infrastructure.Services.Abstract;

public interface IBankDataService
{
    Currency GetDefaultCurrency();
    Task<List<CurrencyRateResponse>?> GetExchangeRates();
}
