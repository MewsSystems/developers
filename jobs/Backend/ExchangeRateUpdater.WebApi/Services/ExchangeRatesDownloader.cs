using ExchangeRateUpdater.WebApi.Services.Interfaces;

namespace ExchangeRateUpdater.WebApi.Services
{
    public class ExchangeRatesDownloader : IExchangeRatesGetter
    {
        private readonly IConfiguration _configuration;
        private readonly IExchangeRatesDownloaderFromURL _exchangeRateDownloaderfromURL;

        public ExchangeRatesDownloader(IConfiguration configuration, IExchangeRatesDownloaderFromURL exchangeRateDownloaderfromURL)
        {
            _configuration = configuration;
            _exchangeRateDownloaderfromURL = exchangeRateDownloaderfromURL;
        }

        public async Task<string> GetRawExchangeRates()
        {
            string? cnbExchangeRatesUrl = _configuration.GetSection("CnbExchangeRatesUrl")?.Value;

            if (string.IsNullOrEmpty(cnbExchangeRatesUrl))
            {
                throw new Exception("Exchange rates source URL is missing in configuration");
            }

            return await _exchangeRateDownloaderfromURL.GetExchangeRatesRawTextFromURL(cnbExchangeRatesUrl);
        }
    }
}
