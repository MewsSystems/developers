using ExchangesRates.Infrastructure.External.CNB.Dtos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRates.Infrastructure.External.CNB
{
    public interface ICnbHttpClient
    {
        Task<ExRateDailyResponse> GetDailyExchangeRatesAsync(string date = null, string lang = "EN");
}

    public class CNBHttpClient : ICnbHttpClient
    {
        private readonly HttpClient _httpClient;

        public CNBHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.cnb.cz/");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        /// <summary>
        /// Gets the daily exchange rates from the Czech National Bank API.
        /// </summary>
        /// <param name="date">Date in yyyy-MM-dd format (optional, defaults to the latest available).</param>
        /// <param name="lang">Language (CZ or EN, default is EN).</param>
        /// <returns>An object containing the list of exchange rates, or null if an error occurred.</returns>
        public async Task<ExRateDailyResponse> GetDailyExchangeRatesAsync(string date = null, string lang = "EN")
        {
            try
            {
                var query = $"?lang={lang}";
                if (!string.IsNullOrEmpty(date))
                    query += $"&date={date}";

                var response = await _httpClient.GetAsync($"/cnbapi/exrates/daily{query}");

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine("Data not found for the specified date.");
                    return null;
                }

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"HTTP error {(int)response.StatusCode}: {response.ReasonPhrase}\n{error}");
                    return null;
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var result = await response.Content.ReadFromJsonAsync<ExRateDailyResponse>(options);
                return result;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Network error: {ex.Message}");
                return null;
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("The request timed out while connecting to the Czech National Bank API.");
                return null;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing JSON response: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return null;
            }
        }
    }
}