using ExchangeRateUpdater.CzechNationalBank.Contracts;
using Refit;

namespace ExchangeRateUpdater.CzechNationalBank;

internal interface ICzechNationalBankClient
{
    [Get("/exrates/daily?lang={language}")]
    Task<GetDailyExchangeRatesResponse> GetDailyExchangeRatesAsync([Query] Language language = Language.EN);
}