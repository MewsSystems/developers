using System.Collections.Generic;

namespace ExchangeRateUpdater.Interfaces
{
    public interface IHttpClientLineReader
    {
        public IAsyncEnumerable<string> ReadLines(string url);
    }
}
