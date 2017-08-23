using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace ExchangeRateUpdater
{
	/// <summary>
	/// European Central Bank exchange rate provider
	/// </summary>
	public class CubExchangeRateProvider : BaseExchangeRateProvider
    {
		public CubExchangeRateProvider(ICacheService cacheService)
			: base("CUBExchangeRateProvider", cacheService)
		{
		}

		protected override Dictionary<string, ExchangeRate> ParseResponce(string response)
		{
			Logger.Trace("Downloaded data will be parsed");

			if (string.IsNullOrEmpty(response))
				throw new System.ArgumentNullException("response");

			var xDoc = XDocument.Parse(response);
			var ns = xDoc.Root.GetDefaultNamespace();
			var cube = xDoc.Root.Element(XName.Get("Cube", ns.NamespaceName));
			var innerCube = cube.Element(XName.Get("Cube", ns.NamespaceName));
			var cubes = innerCube.Elements(XName.Get("Cube", ns.NamespaceName));
			
			var dic = new Dictionary<string, ExchangeRate>();
			foreach(var c in cubes)
			{
				var currency = c.Attribute("currency").Value;
				var rate = decimal.Parse(c.Attribute("rate").Value);
				var exchangeRate = new ExchangeRate(new Currency("EUR"), new Currency(currency), rate);
				dic.Add(currency, exchangeRate);
			}

			return dic;
		}
	}
}
