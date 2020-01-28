using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
		private readonly IDataClient _client;
		private readonly IRateExtractor _rateExtractor;

		/// <summary>
		/// The constructor.
		/// </summary>
		/// <param name="client">The specific data client.</param>
		/// <param name="rateExtractor">The specific rate extractor.</param>
		/// <exception cref="ArgumentNullException"><paramref name="client"/> or <paramref name="rateExtractor"/> is null.</exception>
		public ExchangeRateProvider(IDataClient client, IRateExtractor rateExtractor)
        {
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_rateExtractor = rateExtractor ?? throw new ArgumentNullException(nameof(rateExtractor));
		}

		/// <summary>
		/// The construtor. <see cref="TxtCnbRateExtractor"/> is used as rate extractor and <see cref="HttpDataClient"/> is used as data client.
		/// </summary>
		public ExchangeRateProvider()
			:this(new HttpDataClient(new HttpClient(), "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt")
				 , new TxtCnbRateExtractor()) { }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
			if (currencies == null)
				throw new ArgumentNullException(nameof(currencies));
			if (currencies.Count() == 0)
				return Enumerable.Empty<ExchangeRate>();
			using (Stream content = _client.GetDataAsync().Result)
			using (var reader = new StreamReader(content))
			{
				IDictionary<string, decimal> rates = _rateExtractor.Extract(reader.ReadToEnd());
				return Compose(currencies, rates);
			}
		}

        private IEnumerable<ExchangeRate> Compose(IEnumerable<Currency> currencies, IDictionary<string, decimal> rates)
        {
			var czkCurrency = new Currency("CZK");
			var outputRates = new List<ExchangeRate>();
            foreach (Currency currency in currencies)
            {
                if (rates.TryGetValue(currency.Code, out decimal rate))
                    outputRates.Add(new ExchangeRate(currency, czkCurrency, rate));
            }
            return outputRates;
        }
    }
}
