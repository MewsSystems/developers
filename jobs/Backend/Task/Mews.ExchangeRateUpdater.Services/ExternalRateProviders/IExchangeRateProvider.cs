using ExchangeRateModel = Mews.ExchangeRateUpdater.Services.Models.ExchangeRateModel;

namespace Mews.ExchangeRateUpdater.Services.ExternalRateProviders
{
    public interface IExchangeRateProvider
    {
        public Task<IEnumerable<ExchangeRateModel>> GetExchangeRates();

        public bool CanProvide(string exchangeRateProvider);
    }
}
