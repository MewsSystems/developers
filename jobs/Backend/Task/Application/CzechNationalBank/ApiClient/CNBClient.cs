using Application.CzechNationalBank.ApiClient.Dtos;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Application.CzechNationalBank.ApiClient
{
    public sealed class CNBClient(
        HttpClient httpClient,
        ILogger<CNBClient> logger) : IDisposable, ICNBClient
    {
        public async Task<CNBExRateDailyResponseDto?> GetExRateDailies()
        {
            try
            {
                // may want to consider using Polly for retries
                var resp = await httpClient.GetFromJsonAsync<CNBExRateDailyResponseDto>(
                    CNBApiConfiguration.DailyExchangeRatesEndpoint, new JsonSerializerOptions(JsonSerializerDefaults.Web));
                return resp;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching exchange rates from CNB.");
                return null;
            }
        }

        public void Dispose() => httpClient?.Dispose();
    }
}
