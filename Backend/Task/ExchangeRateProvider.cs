using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
		public string url { get; }
		public Currency targetCurrency { get; }
			
		public ExchangeRateProvider(string targetCurrencyName, string url) 
		{
			targetCurrency = new Currency(targetCurrencyName);
			this.url = url;
		}
		public ExchangeRateProvider() {
			targetCurrency = new Currency("CZK");
			url = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
		}

		/// <summary>
		/// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
		/// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
		/// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
		/// some of the currencies, ignore them.
		/// </summary>
		public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
			var exchangeRates = new List<ExchangeRate>();
			var request = WebRequest.Create(url);
			var response = request.GetResponse();
			using (var responseStream = new StreamReader(response.GetResponseStream())) 
			{
				string line;
				while ((line = responseStream.ReadLine()) != null) 
				{
					var columns = line.Split('|');
					if (columns.Length != 5) continue;

					var sourceCurrency = new Currency(columns[3]);
					if (! decimal.TryParse(columns[4], out decimal value)) continue;
					if (! decimal.TryParse(columns[2], out decimal amount)) continue;
					decimal adjustedValue = value / amount;

					if (currencies.Any( x => x.Code == sourceCurrency.Code)) 
					{
						exchangeRates.Add(new ExchangeRate(sourceCurrency, targetCurrency, adjustedValue));
					}
				}
			}
			return exchangeRates;
        }
    }
}
