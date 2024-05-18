using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Configuration;

namespace ExchangeRateUpdater.Interfaces
{
    public interface ICnbApiClient
    {
        /*
         * Should be a representation of ALL the possible operations that one can do
         * on the CNB Api: https://api.cnb.cz/cnbapi/swagger-ui.html#/
         */
        Task<GetExchangeRatesResponse> GetDailyExchangeRates(DateTime? date, string? lang);
    }
}
