using System.Net;

namespace ExchangeRateUpdater.Services
{
	public class ExchangeRateDownloader
	{
		private readonly string url;

		public ExchangeRateDownloader(string url)
		{
			this.url = url;
		}

		public string Download()
		{
			using (var client = new WebClient())
			{
				return client.DownloadString(url);
			}
		}
	}
}
