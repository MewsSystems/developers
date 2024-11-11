using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
	public interface IExchangeRateApi
	{
		Task<ExchangeRateDitributor> GetLastExchangeRateAsync();
	}

	public class CzechExchangeRateApi : IExchangeRateApi
	{
		
		private readonly HttpClient _httpClient;
		
		public CzechExchangeRateApi()
		{
			_httpClient = new HttpClient()
			{
				BaseAddress = new Uri("https://api.cnb.cz"),
			};
		}
		
		public async Task<ExchangeRateDitributor> GetLastExchangeRateAsync()
		{
			string path = "/cnbapi/exrates/daily?lang=EN";
			
			HttpResponseMessage response = await _httpClient.GetAsync(path);

			if(response.IsSuccessStatusCode)
			{
				ExrateResponseDto responseDto = await response.Content.ReadFromJsonAsync<ExrateResponseDto>();
				
				List<ExchangeRate> rates = new();
				DateTime lastUpdate = DateTime.MaxValue;
				
				responseDto.Rates.ForEach(rateDto => 
				{
					rates.Add(ToExchangeRate(rateDto));
					lastUpdate = new DateTime(Math.Min(lastUpdate.Ticks, rateDto.ValidFor.Ticks));
				});
				
				return new ExchangeRateDitributor(rates, lastUpdate);
			}
			
			throw await ThrowException(response);
		}
		
		private static async Task<Exception> ThrowException(HttpResponseMessage errorResponse)
		{
			try
			{
				ErrorResponse errorDto = await errorResponse.Content.ReadFromJsonAsync<ErrorResponse>();
				return new ExchangeRateApiException($"Unexpected {errorResponse.StatusCode} response from {errorDto.EndPoint} with error code {errorDto.ErrorCode} cause {errorDto.Description}");
			}
			catch(JsonException e)
			{
				return new ExchangeRateApiException($"Unexpected {errorResponse.StatusCode} response.", e);	
			}
		}
		
		private static ExchangeRate ToExchangeRate(ExRateDto dto)
		{
			decimal value = dto.Rate/dto.Amount;
			return new ExchangeRate(new Currency(dto.CurrencyCode), new("CZK"), value, dto.ValidFor);
		}
		
		private record ExRateDto(int Amount, string Country, string Currency, string CurrencyCode, int Order, decimal Rate, DateTime ValidFor);
		private record ExrateResponseDto(List<ExRateDto> Rates);
		private record ErrorResponse(string Description, string EndPoint, string ErrorCode, string MessageId);

	}
	
	public class ExchangeRateApiException: Exception
	{
		public ExchangeRateApiException(string message): base(message){}
		public ExchangeRateApiException(string message, Exception innerException): base(message, innerException){}
	}

}