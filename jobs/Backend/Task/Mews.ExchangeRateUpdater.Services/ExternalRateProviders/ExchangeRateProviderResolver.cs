using Microsoft.Extensions.Configuration;

namespace Mews.ExchangeRateUpdater.Services.ExternalRateProviders
{
    /// <summary>
    /// This purpose of this class is to tell which ExchangeRateProvider instance to use
    /// </summary>
    public class ExchangeRateProviderResolver : IExchangeRateProviderResolver
    {
        private readonly IEnumerable<IExchangeRateProvider> _exchangeRateProviders;
        private readonly string _exchangeRateProvider;

        public ExchangeRateProviderResolver(IEnumerable<IExchangeRateProvider> exchangeRateProviders, IConfiguration configuration)
        {
            _exchangeRateProviders = exchangeRateProviders;
            _exchangeRateProvider = configuration["ExchangeRateProvider"] ?? "CNB";
        }

        public IExchangeRateProvider GetExchangeRateProvider()
        {
            var exchangeRateProvider = _exchangeRateProviders.First(x => x.CanProvide(_exchangeRateProvider));

            return exchangeRateProvider;
        }
    }
}
