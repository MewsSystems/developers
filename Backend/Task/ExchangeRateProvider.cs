using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ExchangeRateUpdater
{
	public class ExchangeRateProvider
	{
		private const string CNB_URL = "http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";
		private const string DEFAULT_CURRCODE = "CZK";

		/// <summary>
		/// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
		/// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
		/// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
		/// some of the currencies, ignore them.
		/// </summary>
		public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
		{
			if (currencies == null || currencies.Count() == 0)
				return Enumerable.Empty<ExchangeRate>();

			Dictionary<string, decimal> rates = getRatesFromWeb();

			if (rates == null || rates.Count==0)
				return Enumerable.Empty<ExchangeRate>();

			List<ExchangeRate> result = new List<ExchangeRate>();

			foreach(var currency in currencies)
			{
				var currencyData = rates.Where(d => d.Key.Equals(currency.Code, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

				if (currencyData.Key != null)
				{
					result.Add(new ExchangeRate(new Currency(currencyData.Key), new Currency(DEFAULT_CURRCODE), currencyData.Value));
				}
				
			}
			
			return result;
        }

		private Dictionary<string,decimal> getRatesFromWeb()
		{
			Dictionary<string, decimal> result = new Dictionary<string, decimal>();
			string rawData;

			try
			{
				using (var client = new WebClient())
				{
					client.Encoding = Encoding.UTF8;
					rawData = client.DownloadString(CNB_URL);
				}

				if (!string.IsNullOrWhiteSpace(rawData))
				{
					string[] rates = rawData.Split(new char[] {'\n' }, StringSplitOptions.RemoveEmptyEntries);

					//line 0 and 1 does not contain valid data (contains date, order number and column description)
					if (rates.Length > 2)
					{
						for (int i = 2; i < rates.Length; i++)
						{
							string[] currencyData = rates[i].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

							decimal currencyValue = Convert.ToDecimal(currencyData[4]) / Convert.ToInt32(currencyData[2]);

							result.Add(currencyData[3], currencyValue);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message);
			}
			
			return result;
		}


    }
}
