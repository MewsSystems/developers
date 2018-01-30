using ExchangerateLab.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateLabProvider : ISpecificExchangeRateProvider
    {
        private const string _ProviderName = "Exchange rate lab";
        private const string _BaseUrl = "http://api.exchangeratelab.com/";
        private const string ApiKey = "C8BF1994DCC7E6AFD17347241C163CF9"; //limited! 50 requests per day!

        private string baseCurrencyCode = "USD";
        private Currency baseCurrency = null; 

        public ExchangeRateLabProvider()
        {
            baseCurrency = new Currency(baseCurrencyCode);
            if (Currency.IsNullOrEmpty(BaseCurrency) || string.IsNullOrEmpty(BaseURL) || string.IsNullOrEmpty(ProviderName)) throw new Exception(Res.InternalErrorProviderExRateLab);
        }
        /// <summary>
        /// Base currency. It can be changed inside class. It depends on received data from provider.
        /// </summary>
        public Currency BaseCurrency
        {
            get
            {
                return (baseCurrency);
            }
            private set
            {
                if(Currency.IsNullOrEmpty(value)) throw new Exception(Res.CurrencyShouldBeSet);
                if (!value.IsEqual(baseCurrency))
                {
                    baseCurrency = value;
                }
            }
        }

        /// <summary>
        /// Base URL. 
        /// </summary>
        public string BaseURL
        {
            get
            {
                return (_BaseUrl);
            }
        }

        /// <summary>
        /// Provider name
        /// </summary>
        public string ProviderName
        {
            get
            {
                return (_ProviderName);
            }
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        /// <param name="currencies">Currencies for output exchange rate. It can't be empty or null</param>
        /// <returns>Returns exchange rate step by step.</returns>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies == null || currencies.Count() <= 0) throw new Exception(Res.CurrenciesShouldBeSetForGettingER);
            ExchangeRateResponse e = ExchangeRateLabHelper.GetRates(ApiKey);
            if(e==null || e.rates==null || e.rates.Count<=0) throw new Exception(Res.SourceDataIsEmpty);
            if (string.IsNullOrEmpty(e.baseCurrency)) throw new Exception(Res.ParsingError);
            var nBaseCur = new Currency(e.baseCurrency);
            BaseCurrency = nBaseCur;
            if (Currency.IsNullOrEmpty(BaseCurrency)) throw new Exception(Res.InternalErrorProviderExRateLab);
            foreach (var targetCurrency in currencies)
            {
                if (Currency.IsNullOrEmpty(targetCurrency)) throw new Exception(Res.EmptyCurrencyCode);//make exception if empty currency
                foreach (var rate in e.rates)
                {
                    if (rate == null || string.IsNullOrEmpty(rate.to)) continue;//miss unsupported currency
                    var supportedTargetCurrency = new Currency(rate.to);
                    if (Currency.IsNullOrEmpty(supportedTargetCurrency)) continue;//miss unsupported currency
                    if (supportedTargetCurrency.IsEqual(targetCurrency)) //if supports then add
                    {
                        yield return (new ExchangeRate(BaseCurrency, targetCurrency, (decimal)rate.rate));
                    }
                }
            }
        }

    }

}
