using System;
using System.Net;

namespace ExchangeRateUpdater.ExchangeRateProviders
{
	public interface IContentDownloader
	{
		string[] DownloadLines(string url);
	}

	public class ContentDownloader : IContentDownloader
	{
		public string[] DownloadLines(string url)
		{
			using (var wc = new WebClient())
			{
				wc.Encoding = System.Text.Encoding.UTF8;
				return wc.DownloadString(url).Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
			}
		}
	}
}
