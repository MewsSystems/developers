using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProviderFactory
    {
        private Dictionary<ExchangeRateProviderType, IExchangeRateProvider> exchangeRateProviders { get; set; }
        public ExchangeRateProviderFactory()
        {
            //get all exchange rate providers that implements the corresponding interface
            //in this scenario we could go with some switch, but I wanted to demonstrate this solution that does not need the update of switch statement everytime when new provider is added
            exchangeRateProviders = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(IExchangeRateProvider).IsAssignableFrom(t) && !t.IsInterface)
                .Select(t => Activator.CreateInstance(t) as IExchangeRateProvider)
                .ToDictionary(f => f.Type);
        }

        public IExchangeRateProvider GetExchangeRateProvider(ExchangeRateProviderType type)
        {
            return exchangeRateProviders[type];
        }
    }
}
