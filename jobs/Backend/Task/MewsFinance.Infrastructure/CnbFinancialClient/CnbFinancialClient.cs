using AutoMapper;
using MewsFinance.Application.Clients;
using MewsFinance.Domain.Models;
using MewsFinance.Infrastructure.CnbFinancialClient.Mappings;
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

        public string TargetCurrencyCode => "CZK";

        public async Task<Response<IEnumerable<ExchangeRate>>> GetExchangeRates(DateTime date)
        {
            var dateStr = date.ToString(@"yyyy-MM-dd");
            string url = $"exrates/daily?date={dateStr}";

            var response = await _httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode
                || string.IsNullOrWhiteSpace(content))
            {
                var message = response.ReasonPhrase ?? string.Empty;

                return CreateEmptyDataResponse(response.IsSuccessStatusCode, message);                
            }  

            var apiResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<CnbExchangeRateResponse>(content);

            if(apiResponse == null)
            {
                var message = string.Empty;

                return CreateEmptyDataResponse(response.IsSuccessStatusCode, message);
            }

            var exchangeRates = _mapper.Map<IEnumerable<ExchangeRate>>(
                apiResponse.Rates,
                opt => opt.Items[MappingConstants.TargetCurrencyCode] = TargetCurrencyCode);

            return CreateDataResponse(
                exchangeRates: exchangeRates, 
                isSuccess: true, 
                message: string.Empty);
        }

        private static Response<IEnumerable<ExchangeRate>> CreateEmptyDataResponse(
            bool isSuccess, string message)
        {
            return new Response<IEnumerable<ExchangeRate>>(
                    data: Enumerable.Empty<ExchangeRate>(),
                    isSuccess: isSuccess,
                    message: message);
        }

        private static Response<IEnumerable<ExchangeRate>> CreateDataResponse(
            IEnumerable<ExchangeRate> exchangeRates, bool isSuccess, string message)
        {
            return new Response<IEnumerable<ExchangeRate>>(
                    data: exchangeRates,
                    isSuccess: isSuccess,
                    message: message);
        }
    }
}
