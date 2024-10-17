using ExchangeRateUpdater.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank
{
    public class CzechNationalBankExchangeRateApiClient(
        HttpClient httpClient, 
        ILogger<CzechNationalBankExchangeRateApiClient> logger) : IExchangeRateApiClient
    {
        public async Task<IReadOnlyList<ApiExchangeRate>> GetDailyExchangeRatesAsync()
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<CzechNationalBankExchangeRatesResponse>($"exrates/daily?lang=EN");

                return response.Rates.Select(x => new ApiExchangeRate(x.CurrencyCode, x.Rate)).ToArray();
            }
            catch(Exception e)
            {
                logger.LogError(e, "Failed to get daily exchange rates from bank API");
                throw;
            }            
        }
    }
}
