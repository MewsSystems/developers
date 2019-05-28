using System;
using System.Net.Http;

namespace ExchangeRateUpdater
{
    public interface IHttpClientFactory
    {
        HttpClient Create();
    }
}
