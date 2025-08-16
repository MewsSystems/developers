using ExchangeRateUpdater.Domain.Types;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.Models;

namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank.ApiClients;

public interface IApiClient
{
    Task<NonNullResponse<IEnumerable<RateDto>>> GetCentralBankRates(string date);
    Task<NonNullResponse<IEnumerable<RateDto>>> GetOtherCurrenciesRates(string date);
}