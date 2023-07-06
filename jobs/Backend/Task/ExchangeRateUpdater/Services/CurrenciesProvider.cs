using System.Collections.Generic;
using ExchangeEntities;
using ExchangeRateUpdater.Interfaces;
using Microsoft.Extensions.Configuration;
using Utils.Extensions;

namespace ExchangeRateUpdater.Services
{
    public class CurrenciesProvider : ICurrenciesProvider
	{
        private readonly IConfiguration _configuration;

        public CurrenciesProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<Currency> GetCurrenciesFromConfig()
        {
            var currenciesAsEnumerable = _configuration.GetRequiredValue<List<string>>("Currencies");

            return CreateCurrenciesEnumerable(currenciesAsEnumerable);   
        }

        private static IEnumerable<Currency> CreateCurrenciesEnumerable(IEnumerable<string> currenciesAsEnumerable)
        {
            var currencies = new List<Currency>();

            foreach (var currency in currenciesAsEnumerable)
            {
                currencies.Add(new Currency(currency));
            }

            return currencies;
        }
    }
}

