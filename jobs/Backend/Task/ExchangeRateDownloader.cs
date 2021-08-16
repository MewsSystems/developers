using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater
{
    public sealed class ExchangeRateDownloader: IExchangeRateDownloader
    {
        readonly ExchangeRateDownloaderOptions options;

        public ExchangeRateDownloader(IOptions<ExchangeRateDownloaderOptions> options)
        {
            this.options = options.Value;
        }

        public async Task<string> DownloadExchangeRatesAsync(CancellationToken cancellation)
        {
            try
            {
                using var httpClient = new HttpClient();
                using var response = await httpClient.GetAsync(options.DownloadUri, cancellation);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new ExchangeRateDownloadException($"Error when downloading exchange rate data from {options.DownloadUri}.", ex);
            }
        }
    }

    public sealed record ExchangeRateDownloaderOptions(Uri? downloadUri = default)
    {
        public Uri DownloadUri { get; init; } = downloadUri ??
            new Uri("https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt");
    }

    public class ExchangeRateDownloadException : Exception
    {
        public ExchangeRateDownloadException(string message, Exception inner) : base(message, inner)
        {           
        }
    }
}