using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank
{
    public class CzechNationalBankApiClient(
        HttpClient httpClient, 
        ILogger<CzechNationalBankApiClient> logger) : ICzechNationalBankApiClient
    {
        public string TargetCurrencyCode => "CZK";

        public async Task<IReadOnlyList<BankApiExchangeRate>> GetDailyExchangeRatesAsync()
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<GetDailyExchangeRatesResponse>($"exrates/daily?lang=EN");

                return response.Rates.Select(x => new BankApiExchangeRate(x.CurrencyCode, x.Rate)).ToArray();
            }
            catch(Exception e)
            {
                logger.LogError(e, "Failed to get daily exchange rates from bank API");
                throw;
            }            
        }
    }
}
