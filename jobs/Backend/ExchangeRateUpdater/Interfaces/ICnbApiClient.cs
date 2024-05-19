using ExchangeRateUpdater.Clients;

namespace ExchangeRateUpdater.Interfaces
{
    public interface ICnbApiClient : IExternalApiClient
    {
        /*
         * TODO: Should be a representation of ALL the possible operations that one can do
         * on the CNB Api: https://api.cnb.cz/cnbapi/swagger-ui.html#/
         */
        Task<GetExchangeRatesResponseDto> GetDailyExchangeRates(DateTime? date, string? lang);
    }
}
