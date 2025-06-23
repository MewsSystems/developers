using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Providers
{
    public class CurrencyProvider : ICurrencyProvider
    {
        private readonly IEnumerable<Currency> _currencies;

        public CurrencyProvider(IOptions<ExchangeRateUpdaterConfiguration> configuration)
        {
            if (configuration?.Value?.TargetCurrencies == null)
            {
                throw new ArgumentNullException(nameof(configuration), "TargetCurrencies section is missing in the configuration.");
            }

            _currencies = configuration.Value.TargetCurrencies.Select(currency => new Currency(currency)).ToList();
        }

        public IEnumerable<Currency> Get()
        {
            return _currencies;
        }
    }
}
