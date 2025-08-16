using ExchangeRateProvider.Implementations.CzechNationalBank.Models;

namespace ExchangeRateProvider.Implementations.CzechNationalBank;

internal interface ICzechNationalBankApi
{
    Task<ExRateDailyResponse> GetExratesDaily(DateTimeOffset date);
}