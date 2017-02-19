using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
		const string _query = "http://query.yahooapis.com/v1/public/yql?q=select * from yahoo.finance.xchange where pair in ({0})&env=store://datatables.org/alltableswithkeys";

		XElement LoadPage(Uri uri)
		{
			var req = WebRequest.Create(uri);
			using (var resp = req.GetResponse())
			{
				using (var stream = resp.GetResponseStream())
				{
					return XDocument.Load(stream).Root;
				}
			}
		}

		IEnumerable<string> JoinCurrencies(IEnumerable<Currency> currencies)
		{
			foreach (var c1 in currencies)
			{
				foreach (var c2 in currencies)
				{
					if (c1.Code != c2.Code) yield return $"\"{c1.Code}{c2.Code}\"";
				}
			}
		}

		string CreateQuery(IEnumerable<Currency> currencies)
		{
			var joined = JoinCurrencies(currencies);
			return String.Format(_query, String.Join(",", joined));
		}

		IEnumerable<ExchangeRate> ParseExchangeRates(XElement page)
		{
			return page.Descendants()
						.Where(el => el.Name.LocalName == "rate")
						.Select(el =>
								new
								{
									Currencies = el.Element("Name")
												   .Value
												   .Split('/')
												   .Select(x => new Currency(x))
												   .ToArray(),
									Rate = el.Element("Rate").Value
								})
				       .Where(x => x.Rate != "N/A")
					   .Select(x => new ExchangeRate(x.Currencies[0], x.Currencies[1], Convert.ToDecimal(x.Rate)));

		}

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
			var query = CreateQuery(currencies);
			var page = LoadPage(new Uri(query));
			return ParseExchangeRates(page);
        }
	}
}
