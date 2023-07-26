using ExchangeRateUpdater.ApiClient.Client.ExchangeDaily;
using ExchangeRateUpdater.ApiClient.Common;

namespace ExchangeRateUpdater.ApiClient.Client
{
    public interface ICnbClient
    {
        /// <summary>
        /// This method make a call to  cnb exchange daily url: cnbapi/exrates/daily
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="language">The lenguage CZ/EN</param>
        /// <returns></returns>
        Task<ExchangeDailyCommand> GetExchangesDaily(DateTime date, Language language);
    }
}
