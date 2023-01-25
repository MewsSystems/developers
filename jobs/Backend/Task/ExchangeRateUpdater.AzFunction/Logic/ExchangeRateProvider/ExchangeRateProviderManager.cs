using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.AzFunction.Logic.ExchangeRateProvider
{
    /// <summary>
    /// Creating a Façade just in order to easily allow new providers in the future.
    /// Currently we only need a provider to CNB and to a specific API, but maybe in the future we need to get the details from another place.
    /// </summary>
    public class ExchangeRateProviderManager: IExchangeRateProviderManager
    {
        private readonly IHttpClientFactory _httpClient;
        
        public ExchangeRateProviderManager(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public IExchangeRateProvider GetExchangeRateProvider(string currency)
        {
            if (currency.ToUpper() == "CZK")
            {
                return new ExchangeRateProvider(_httpClient, currency);
            }
            //more providers in the future
            throw new NotImplementedException();
        }
    }
}
