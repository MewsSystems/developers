using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Mews.Backend.Task.Core
{
    public class TextFileExchangeRateParser : IExchageRateParser
    {
        private readonly string _fileUrl;

        public TextFileExchangeRateParser(string fileUrl)
        {
            _fileUrl = fileUrl;
        }

        public async Task<IEnumerable<ExchangeRateDto>> GetExchangeRatesAsync()
        {
            using (var webClient = new WebClient())
            {
                var result = await webClient.DownloadStringTaskAsync(new Uri(_fileUrl, UriKind.Absolute));

                return result
                    .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                    .Skip(2)
                    .Select(x =>
                    {
                        var values = x.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                        return new ExchangeRateDto
                        {
                            Country = values[0],
                            Currency = values[1],
                            Amount = Convert.ToInt32(values[2]),
                            Code = values[3],
                            Rate = Convert.ToDecimal(values[4])
                        };
                    });
            }
        }
    }
}
