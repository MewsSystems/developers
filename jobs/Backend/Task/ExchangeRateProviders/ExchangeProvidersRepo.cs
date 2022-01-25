using ExchangeRateUpdater.CoreClasses;
using ExchangeRateUpdater.ExchangeRateProviders.QuotesParsers;
using ExchangeRateUpdater.ExchangeRateProviders.QuotesProviders;
using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.ExchangeRateProviders
{
    public static class ExchangeProvidersRepo
    {
        static Dictionary<Currency, ExchangeRateProvider> _exchangeProviders = new Dictionary<Currency, ExchangeRateProvider>();
        static ExchangeProvidersRepo()
        {
            _exchangeProviders.Add(new Currency("CZK"), new CnbExchangeRateProvider(new CnbQuotesHttpProvider(new WebProxyProvider()), new CnbQuotesTextParser()));
        }

        public static ExchangeRateProvider GetProviderByCurrency(Currency currency)
        {
            if (_exchangeProviders.ContainsKey(currency)) 
                return _exchangeProviders[currency];

            throw new Exception($"No provider found for currency {currency}");
        }
    }
}
