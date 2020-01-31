using System;
using System.Net;
using System.Text;

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
					var data = client.DownloadData(url);
					return Encoding.UTF8.GetString(data);					
			}
		}
	}
}
