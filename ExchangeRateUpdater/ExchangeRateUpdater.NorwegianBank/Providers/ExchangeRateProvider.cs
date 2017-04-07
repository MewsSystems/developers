using ExchangeRateUpdater.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater.NorwegianBank.Providers
{
    public abstract class ExchangeRateProvider : IExchangeRateProvider
    {
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies == null)
                throw new ArgumentNullException("Currencies is null");
            if (!currencies.Any())
                return Enumerable.Empty<ExchangeRate>();

            var currencyCodes = currencies.Select(x => x.Code);

            string response = GetStringResponse(currencyCodes);

            IEnumerable<ExchangeRate> result = Convert(response, currencyCodes);

            return result;
        }

        /// <summary>
        /// Url of the currency exchange rates resource
        /// </summary>
        public abstract string Url { get; }

        /// <summary>
        /// Make a request to currency exchange rates resource and return a string with response
        /// </summary>
        /// <param name="currencyCodes">List of currency codes</param>
        /// <returns>string with response</returns>
        protected virtual string GetStringResponse(IEnumerable<string> currencyCodes)
        {
            string jsonString = null;

            using (WebClient client = new WebClient())
            {
                jsonString = client.DownloadString(Url);
            }

            return jsonString;
        }

        /// <summary>
        /// Convert response string to list of ExchangeRate
        /// </summary>
        /// <param name="response">String with response</param>
        /// <returns></returns>
        protected abstract IEnumerable<ExchangeRate> Convert(string response, IEnumerable<string> currencyCodes);
    }
}
