using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank
{
    public class CzechNationalBankApiClient(HttpClient httpClient) : ICzechNationalBankApiClient
    {
        private const string DateParameterFormat = "yyyy-MM-dd";

        public string TargetCurrencyCode => "CZK";

        public async Task<ExchangeRate[]> GetDailyExchangeRatesAsync(DateTime date)
        {
            var dateHttpParameter = date.ToString(DateParameterFormat);

            var response = await httpClient.GetFromJsonAsync<GetDailyExchangeRatesResponse>($"exrates/daily?date={dateHttpParameter}&lang=EN");

            return response.Rates.Select(x => new ExchangeRate(x.CurrencyCode, x.Rate)).ToArray();
        }
    }
}
