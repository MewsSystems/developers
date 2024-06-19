
using ExchangeRateUpdater.API.CNB.Model.Responses;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System;

namespace ExchangeRateUpdater.API.CNB
{
    public static class CNBApiClient
    {
        private const string API_URL = "https://api.cnb.cz/cnbapi";
        private const string EX_RATES = "exrates";
        private const string DAILY_RATES = "daily";
        public static async Task<ExRateDailyResponse> GetDailyRates(string lang = "EN", DateTime? date = null)
        {
            using var hc = new HttpClient();
            var query = DAILY_RATES + $"?lang={lang}";

            if (date.HasValue)
                query += $"date={date.Value:yyyy-MM-dd}";

            var response = await hc.GetAsync(string.Join("/", API_URL, EX_RATES, query));

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ExRateDailyResponse>();
        }

    }
}
