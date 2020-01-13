using ExchangeRateUpdaterV2.Contracts;
using ExchangeRateUpdaterV2.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;

namespace ExchangeRateUpdaterV2.Providers
{
    public class CnbExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IEnumerable<Currency> _currencies;

        private const string _uri = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";

        private static readonly NumberFormatInfo _numberFormatInfo = new NumberFormatInfo { NumberDecimalSeparator = "," };

        public CnbExchangeRateProvider(IHttpClientFactory httpClientFactory, ICollection<Currency> currencies)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _currencies = currencies ?? throw new ArgumentNullException(nameof(currencies));
        }

        public string Name { get; private set; } = "Czech National Bank";

        public async IAsyncEnumerable<ExchangeRate> GetExchangeRates()
        {
            // send a GET request
            var responseStream = await _httpClientFactory.CreateClient().GetStreamAsync(_uri);

            // process response and make a dictionary for the later usage
            var rates = XElement.Load(responseStream)
                .Elements()
                .SelectMany(_ => _.Elements())
                .ToDictionary(_ => 
                // the key is a currency code
                _.Attribute(XName.Get("kod")).Value, 
                // value is a currency rate without dependency on the quantity
                _ => decimal.Parse(_.Attribute(XName.Get("kurz")).Value, _numberFormatInfo) / int.Parse(_.Attribute(XName.Get("mnozstvi")).Value));

            // iterate over currency pairs
            foreach (var currencyPair in _currencies.SelectMany(source => 
                _currencies
                // skip currencies where the source is equal to the target
                .Where(target => source.Code != target.Code)
                .Select(target => new { Source = source, Target = target })))
            {
                // no rate available for at least one currency
                if (!(rates.TryGetValue(currencyPair.Source.Code, out var sourceRate) && rates.TryGetValue(currencyPair.Target.Code, out var targetRate)))
                {
                    continue;
                }

                yield return new ExchangeRate(currencyPair.Source, currencyPair.Target, sourceRate / targetRate);
            }
        }
    }
}
