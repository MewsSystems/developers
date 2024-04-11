using AutoMapper;
using MewsFinance.Application.Clients;
using MewsFinance.Domain.Models;
using System.Net.Http.Headers;

namespace MewsFinance.Infrastructure.CnbFinancialClient
{
    public class CnbFinancialClient : IFinancialClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public CnbFinancialClient(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.cnb.cz/cnbapi/");
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            _mapper = mapper;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(DateTime date)
        {
            var dateStr = date.ToString(@"yyyy-MM-dd");
            string url = $"exrates/daily?date={dateStr}";

            var response = await _httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(content, null, response.StatusCode);
            }            

            if (string.IsNullOrWhiteSpace(content))
            {              
                return Enumerable.Empty<ExchangeRate>();
            }

            var apiResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<CnbExchangeRateResponse>(content);

            if(apiResponse == null)
            {
                return Enumerable.Empty<ExchangeRate>();
            }

            var exchangeRates = _mapper.Map<IEnumerable<ExchangeRate>>(apiResponse.Rates);

            return exchangeRates;
        }
    }
}
