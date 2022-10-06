using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Common.Http
{
    public interface IHttpWrapper
    {
        Task<string> HttpGet(string request, TimeSpan? timeout = null);
        Task<string> HttpPost(string request, IEnumerable<KeyValuePair<string, string>> postParameters, TimeSpan? timeout = null);
    }
}
