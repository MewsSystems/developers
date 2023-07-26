using ExchangeRateUpdater.Features.Services.ExchangeRagesDaily.V1;

namespace ExchangeRateUpdater.Features.Services
{
    public interface IExchangeRateService
    {

        /// <summary>
        /// Given a list of currencies it obtains the exchanges rates by exchange provider
        /// </summary>
        /// <param name="currencies">The list of currencies</param>
        /// <returns></returns>
        Task<IEnumerable<ExchangeRateModel>> GetExchangeRates(IEnumerable<CurrencyModel> currencies);
    }
}
