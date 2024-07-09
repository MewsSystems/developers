using System.Threading.Tasks;
using System;
using System.Net.Http;
using ExchangeRateUpdater.Lib.Shared;

namespace ExchangeRateUpdater.Lib.v1CzechNationalBank.ExchangeRateProvider
{
    public class ExchangeRateHttpClient : IExchangeRateHttpClient
    {
        private readonly IExchangeRateProviderSettings _settings;
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public ExchangeRateHttpClient(
            IExchangeRateProviderSettings settings,
            HttpClient httpClient,
            ILogger logger
        )
        {
            _settings = settings;
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Get The Latest Exchange Rate from Czech National Bank for the target currency
        /// </summary>
        /// <param name="currency"></param>
        /// <returns>
        /// async provider specific exchange rate
        /// </returns>
        public async Task<ProviderExchangeRate> GetCurrentExchangeRateAsync(Currency currency)
        {
            _logger.Info($"[{currency}] Requesting Exchange Rate File For Currency Code");

            string requestUrl = BuildRequestUrl(currency);
            string content = string.Empty;

            try
            {
                // Download the txt file
                content = await _httpClient.GetStringAsync(requestUrl);
                _logger.Info($"[{currency}] File content downloaded successfully");
#if DEBUG
                // if were debugging, output the content to the console
                _logger.Info($"[{currency}] Returned Content\n{content}");
#endif
            }
            catch (HttpRequestException hre)
            {
                // Handle HTTP-specific errors
                _logger.Error($"[{currency}] HTTP Request error: {hre.Message}");
            }
            catch (TaskCanceledException tce)
            {
                // Handle timeout and other cancellation-related issues
                _logger.Error($"[{currency}] Request timed out or was canceled: {tce.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                _logger.Error($"[{currency}] An error occurred: {ex.Message}");
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                _logger.Warn($"[{currency}] No content received for currency code, we'll just ignore it");
                return null;
            }

            return ProviderExchangeRate.Deserialize(content);
        }

        /// <summary>
        /// Build a Request Url to get the current months exchange file
        /// </summary>
        /// <param name="currency"></param>
        /// <returns>
        /// Url of Czech National Bank file endpoint
        /// </returns>
        private string BuildRequestUrl(Currency currency)
        {
            (DateTime firstDayOfMonth, DateTime lastDayOfMonth) = GetMonthDateRange();
            return string.Format(_settings.SourceUrl, firstDayOfMonth, lastDayOfMonth, currency);
        }

        /// <summary>
        /// Gets the first and last day of the current month.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>
        /// The first and last day of the current month as DateTime
        /// </returns>
        private (DateTime fromDate, DateTime toDate) GetMonthDateRange(
            DateTime? dateTime = null
            )
        {
            //get the first and last day of the month
            var date = dateTime ?? DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            return (firstDayOfMonth, lastDayOfMonth);
        }
    }

}