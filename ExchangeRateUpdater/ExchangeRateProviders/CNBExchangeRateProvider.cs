using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater.ExchangeRateProviders
{
	public class CNBExchangeRateProvider : IExchangeRateProvider
	{
		private const string DateFormat = "dd.MM.yyyy";
		private static readonly Currency TargetCurrency = new Currency("CZK");

		public IEnumerable<ExchangeRate> GetExchangeRates(DateTime date)
		{
			return
				new[] { BuildMainUrl(date), BuildOthersUrl(date) }
				.SelectMany(GetExchangeRates);
		}

		private static IEnumerable<ExchangeRate> GetExchangeRates(string url)
		{
			return
				DownloadLines(url)
					.Skip(2)
					.Select(ParseExchangeRate);
		}

		private static ExchangeRate ParseExchangeRate(string line)
		{
			var parts = line.Split('|');
			var code = parts[3];
			var exchangeRate = decimal.Parse(parts[4]) / int.Parse(parts[2]);

			return new ExchangeRate(new Currency(code), TargetCurrency, exchangeRate);
		}

		private static string BuildMainUrl(DateTime date)
		{
			const string ExchangeRatesUrlFormat = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt?date={0}";
			return string.Format(ExchangeRatesUrlFormat, date.ToString(DateFormat));
		}

		private static string BuildOthersUrl(DateTime date)
		{
			const string ExchangeRatesUrlFormat = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_ostatnich_men/kurzy.txt?mesic={0}&rok={1}";
			return string.Format(ExchangeRatesUrlFormat, date.Month, date.Year);
		}

		private static string[] DownloadLines(string url)
		{
			using (var wc = new WebClient())
			{
				wc.Encoding = System.Text.Encoding.UTF8;
				return wc.DownloadString(url).Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
			}
		}
	}
}
