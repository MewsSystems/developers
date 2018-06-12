using System;

namespace ExchangeRateUpdater
{
    public interface IWebClientWrapper
    {
        string DownloadString(Uri address);
    }
}