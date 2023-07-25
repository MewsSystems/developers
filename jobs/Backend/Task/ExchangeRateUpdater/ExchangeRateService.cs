using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
   public interface IExchangeRateService
   {
      Task<ExchangeRateServiceResponse> Get();
   }

   public class ExchangeRateService : IExchangeRateService
   {
      private readonly HttpClient _httpClient;

      public ExchangeRateService(HttpClient httpClient)
      {
         _httpClient = httpClient;
         _httpClient.BaseAddress = new Uri("https://api.cnb.cz/cnbapi");
      }

      public async Task<ExchangeRateServiceResponse> Get()
      {
         var response = await _httpClient.GetAsync("exrates/daily");
         response.EnsureSuccessStatusCode();

         var content = await response.Content.ReadAsStringAsync();

         return JsonSerializer.Deserialize<ExchangeRateServiceResponse>(content);
      }
   }
}