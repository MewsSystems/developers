using System;

namespace ExchangeRateUpdater.Models
{
    public class ExchangeRateConfiguration(string baseUrl)
    {
        public Uri BaseUrl { get; set; } = new Uri(baseUrl);
    }
}
