using API.Interfaces;

namespace API.Factory
{
    public class ExchangeRateProviderFactory
    {
        private readonly Dictionary<string, Type> _providerTypeMap;
        private readonly IHttpClientFactory _httpClientFactory;

        public ExchangeRateProviderFactory(IHttpClientFactory httpClientFactory, Dictionary<string, Type> providerTypeMap)
        {
            _providerTypeMap = providerTypeMap;
            _httpClientFactory = httpClientFactory;
        }

        public IExchangeRateProvider? GetExchangeRateProvider(string providerIdentifier)
        {
            if (_providerTypeMap.TryGetValue(providerIdentifier, out var serviceType))
            {
                var httpClient = _httpClientFactory.CreateClient();
                return Activator.CreateInstance(serviceType, httpClient) as IExchangeRateProvider;
            }
            else
            {
                throw new ArgumentException("Unsupported exchange rate provider type.");
            }
        }
    }
}
