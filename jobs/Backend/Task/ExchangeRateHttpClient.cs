using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ExchangeRateUpdater
{
    public class ExchangeRateHttpClient
    {
        private const string CommonExchangeRateUrl = "https://api.cnb.cz/cnbapi/exrates/daily";
        private const string OtherExchangeRateUrl = "https://api.cnb.cz/cnbapi/fxrates/daily-month";
        
        private readonly HttpClient _httpClient;

        public ExchangeRateHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    
        public async Task<ExchangeRateResponse> GetCommonRatesAsync(CancellationToken cancellationToken = default)
        {
            var query = GenerateQueryString(CommonExchangeRateUrl);
        
            using var response = await _httpClient.GetAsync(query, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ExchangeRateResponse>(cancellationToken: cancellationToken);
        }
        
        public async Task<ExchangeRateResponse> GetOtherRatesAsync(CancellationToken cancellationToken = default)
        {
            var query = GenerateQueryString(OtherExchangeRateUrl, MonthlyRateHelper.GetDeclarationMonth());
        
            using var response = await _httpClient.GetAsync(query, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ExchangeRateResponse>(cancellationToken: cancellationToken);
        }

        private static string GenerateQueryString(string baseUrl, string yearMonth = null)
        {
            var builder = new UriBuilder(baseUrl);
            var query = HttpUtility.ParseQueryString(builder.Query);

            query["lang"] = "EN";

            if (!string.IsNullOrEmpty(yearMonth))
                query["yearMonth"] = yearMonth;

            builder.Query = query.ToString() ?? string.Empty;
            return builder.ToString();
        }
    }
}