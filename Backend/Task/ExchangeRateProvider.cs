using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Infrastructure;

namespace ExchangeRateUpdater
{
	/// <summary>
	/// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
	/// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
	/// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
	/// some of the currencies, ignore them.
	/// </summary>
	public class ExchangeRateProvider
    {
		private const string cnbUrl = "http://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
		private const string cnbUrlLocal = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";
		private const string cnbUrlLocalOther = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-ostatnich-men/kurzy-ostatnich-men/kurzy.txt";
		private const char lineSeparator = '\n';
		private const int linesToSkip = 2;


		/* 
		 * we have several points providing data about exhange rates 
		 * so we need be efficicent and request them in parallel to reduce our latency		  
		*/
		public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
		{
			HashSet<string> currenciesSet = new HashSet<string>(currencies.Select(c => c.Code));

			IEnumerable<ExchangeRate> result = Enumerable.Empty<ExchangeRate>();

			Task<string> getDataLines(string url) => HttpClientWrapper.GetStringAsync(url);

			foreach (string dataLine in Task.WhenAll(
				getDataLines(cnbUrlLocal),
				getDataLines(cnbUrlLocalOther)).Result)
			{
				result = result.Concat(GetExchangeRatesFromDataLines(currenciesSet, dataLine));
			}

			return result;
		}

		private IEnumerable<ExchangeRate> GetExchangeRatesFromDataLines(HashSet<string> currenciesSet, string dataLines)
		{
			foreach (string line in dataLines
				.Split(new char[] { lineSeparator }, StringSplitOptions.RemoveEmptyEntries)
				.Skip(linesToSkip)
			)
			{
				var exchangeRate = new ExchangeRateCnb(line);

				if (currenciesSet.Contains(exchangeRate.SourceCurrency.Code))
					yield return exchangeRate;
			}
		}
	}
}
