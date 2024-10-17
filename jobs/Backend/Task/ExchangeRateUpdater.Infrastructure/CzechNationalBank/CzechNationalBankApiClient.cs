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
        public string TargetCurrencyCode => "CZK";

        public async Task<IReadOnlyList<BankApiExchangeRate>> GetDailyExchangeRatesAsync()
        {
            var response = await httpClient.GetFromJsonAsync<GetDailyExchangeRatesResponse>($"exrates/daily?lang=EN");

            return response.Rates.Select(x => new BankApiExchangeRate(x.CurrencyCode, x.Rate)).ToArray();
        }
    }
}
