using AutoMapper;
using MewsFinance.Application.Clients;
using MewsFinance.Domain.Models;
using MewsFinance.Infrastructure.CnbFinancialClient.Mappings;
using MewsFinance.Infrastructure.Http;
using Microsoft.Extensions.Logging;

namespace MewsFinance.Infrastructure.CnbFinancialClient
{
    public class CnbFinancialClient : IFinancialClient
    {
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CnbFinancialClient(
            IHttpClientWrapper httpClientWrapper, 
            IMapper mapper,
            ILogger<CnbFinancialClient> logger)
        {
            _httpClientWrapper = httpClientWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public string TargetCurrencyCode => "CZK";

        public async Task<Response<IEnumerable<ExchangeRate>>> GetExchangeRates(DateTime date)
        {
            var dateStr = date.ToString(@"yyyy-MM-dd");
            string url = $"exrates/daily?date={dateStr}";

            try
            {
                var exchangeRateResponse = await GetExchangeRateResponse(url);

                return exchangeRateResponse;
            }
            catch (Exception exc)
            {
                var message = exc.Message;

                _logger.LogError(exc, exc.Message);

                return CreateEmptyDataResponse(
                    isSuccess: false, 
                    message);
            }           
        }

        private async Task<Response<IEnumerable<ExchangeRate>>> GetExchangeRateResponse(string url)
        {
            var response = await _httpClientWrapper.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode
                || string.IsNullOrWhiteSpace(content))
            {
                var message = response.ReasonPhrase ?? string.Empty;

                return CreateEmptyDataResponse(response.IsSuccessStatusCode, message);
            }

            var apiResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<CnbExchangeRateResponse>(content);

            if (apiResponse == null)
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
