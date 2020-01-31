using System;
using ExchangeRateUpdater.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRateUpdater.Tests
{
	[TestClass]
	public class ExchangeRateDownloaderTests
	{
		private const string ExchangeRateUrl = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";

		[TestMethod]
		public void Download_CorrectUrl_SuccessfullyDownloadedCsv()
		{
			var downloader = new ExchangeRateDownloader(ExchangeRateUrl);

			string csv = downloader.Download();

			Assert.IsFalse(string.IsNullOrWhiteSpace(csv));
		}
	}
}
