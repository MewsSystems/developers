using System;
using System.Net;

namespace ExchangeRateUpdater
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }

        DateTime Today { get; }
    }

    public interface IWebClientFactory
    {
        IWebClientProvider Create();
    }

    public interface IWebClientProvider : IDisposable
    {
        string DownloadString(string url);
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;

        public DateTime Today => DateTime.Today;
    }

    public class WebClientFactory : IWebClientFactory
    {
        public IWebClientProvider Create()
        {
            return new WebClientProvider();
        }
    }

    public class WebClientProvider : WebClient, IWebClientProvider
    {
    }
}