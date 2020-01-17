using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;

namespace ExchangeRateUpdater
{
    public class CNBExchangeRateProvider: IExchangeRateProvider
    {
        private readonly string _url = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.xml";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IEnumerable<Currency> _currencies;


        public CNBExchangeRateProvider(IHttpClientFactory httpClientFactory, IEnumerable<Currency> currencies)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _currencies = currencies ?? throw new ArgumentNullException(nameof(currencies));
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates()
        {
            var stream = _httpClientFactory.CreateClient().GetStringAsync(_url).Result;
            XElement root = XElement.Parse(stream);
            var rates = root.Elements().SelectMany(a => a.Elements())
                .ToDictionary(a => a.Attribute(XName.Get("kod")).Value, 
                a => double.Parse(a.Attribute(XName.Get("kurz")).Value.Replace(",",".")) / int.Parse(a.Attribute(XName.Get("mnozstvi")).Value));
            rates.Add("CZK", 1);

            var availableCurrencies = rates.Keys.Intersect(_currencies.Select(a => a.Code).ToList(),StringComparer.OrdinalIgnoreCase).ToList();
            availableCurrencies.Sort();

            foreach(var source in availableCurrencies)
            {
                foreach(var target in availableCurrencies)
                {
                    if (string.Compare(source, target, StringComparison.OrdinalIgnoreCase)==0) continue;
                    if (!rates.TryGetValue(source, out double sourceRate) || !rates.TryGetValue(target, out double targetRate)) continue;
                    yield return new ExchangeRate(new Currency(source), new Currency(target), (decimal)(sourceRate / targetRate));
                }
            }
        }
    }
}
