using ExchangeRateUpdater.Infrastructure.ExchangeRates.CnbApi;
using ExchangeRateUpdater.Model.ExchangeRates;

namespace ExchangeRateUpdater.Infrastructure.ExchangeRates;

public interface IExchangeRateMapper
{
    IEnumerable<ExchangeRate> MapFromCnbExchangeRatesDailyResponse(CnbExchangeRatesDailyResponse response);
}