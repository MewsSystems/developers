using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.ExternalVendors.CzechNationalBank;

namespace ExchangeRateUpdater;

public interface IExchangeRateClient
{
    Task<ExchangeRateDto> GetDailyExchangeRates();
}