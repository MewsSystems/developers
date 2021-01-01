using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace ExchangeRateUpdater
{
    public class ExchangeRateClient : IExchangeRateClient
    {
        private readonly HttpClient httpClient;

        private readonly string[] urls;
        private readonly int amountIndex = 2;
        private readonly int codeIndex = 3;
        private readonly int rateIndex = 4;

        public ExchangeRateClient(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.urls = new[] { configuration.CNB_URL_MAIN, configuration.CNB_URL_OTHER };
        }

        public async IAsyncEnumerable<(int amout, string code, decimal rate)> GetExchanges(IEnumerable<Currency> currencies)
        {
            var currencyCodes = currencies.Select(s => s.Code);
            foreach (var url in urls)
            {
                using (var stream = await httpClient.GetStreamAsync(url))
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        while (streamReader.Peek() >= 0)
                        {
                            var line = await streamReader.ReadLineAsync();

                            var data = line.Split('|');
                            if (data != null &&
                                data.Count() == 5 &&
                                currencyCodes.Contains(data[codeIndex]))
                            {
                                if (int.TryParse(data[amountIndex], out var amount) &&
                                    decimal.TryParse(data[rateIndex], out var rate) &&
                                    !string.IsNullOrEmpty(data[codeIndex]))
                                {
                                    yield return (amount, data[codeIndex], rate);
                                }

                            }
                        }
                    }
                }
            }
        }
    }
}

