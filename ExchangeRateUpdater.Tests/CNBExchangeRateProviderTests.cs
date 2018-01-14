using ExchangeRateUpdater.ExchangeRateProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Tests
{
	[TestClass]
	public class CNBExchangeRateProviderTests
	{
		[TestMethod]
		public void IfGettingExchangeRatesThenParseMainAndOtherRates()
		{
			var downloader = new FakeDownloader(new[] { "EMU|euro|1|EUR|25,520" }, new[] { "Burundi|frank|100|BIF|1,498" });

			var provider = new CNBExchangeRateProvider(downloader);

			var rates = provider.GetExchangeRates(new DateTime(2018, 1, 14)).ToArray();

			Assert.AreEqual(2, rates.Length);

			Assert.AreEqual("EUR", rates[0].SourceCurrency.Code);
			Assert.AreEqual("CZK", rates[0].TargetCurrency.Code);
			Assert.AreEqual(25.52m, rates[0].Value);

			Assert.AreEqual("BIF", rates[1].SourceCurrency.Code);
			Assert.AreEqual("CZK", rates[1].TargetCurrency.Code);
			Assert.AreEqual(0.01498m, rates[1].Value);
		}

		private class FakeDownloader : IContentDownloader
		{
			private readonly string[] mainLines;
			private readonly string[] otherLines;

			public FakeDownloader(string[] mainLines, string[] otherLines)
			{
				this.mainLines = new[] { "header1", "header2" }.Concat(mainLines).ToArray();
				this.otherLines = new[] { "header1", "header2" }.Concat(otherLines).ToArray();
			}

			public string[] DownloadLines(string url)
			{
				if (url.Contains("kurzy_ostatnich_men"))
				{
					return otherLines;
				}

				return mainLines;
			}
		}
	}
}
