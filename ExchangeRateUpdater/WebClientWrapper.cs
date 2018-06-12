using System;
using System.Net;

namespace ExchangeRateUpdater
{
    internal class WebClientWrapper : IWebClientWrapper
    {
        public string DownloadString(Uri address)
        {
            using (var client = new WebClient())
            {
                return client.DownloadString(address);
            }
        }
    }
}