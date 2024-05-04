using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Net.Http.Json;
using System.Collections.Generic;
using ExchangeRateUpdater.Domain.Core.UseCases.CommonModels;
using ExchangeRateUpdater.Tests.Mocks;
using ExchangeRateUpdater.Domain.Core.Clients;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace ExchangeRateUpdater.Tests.Endpoints.V1.ExchangeRates.GetEchangeRates
{
	public class EchangeRateControllerTests : IClassFixture<WebApplicationFactoryMock>
	{
		private readonly HttpClient _client;

		private readonly HttpBankClientWrapperMock _bankClientMock;

		public EchangeRateControllerTests(WebApplicationFactoryMock application)
		{
			_client = application.CreateClient();
			_bankClientMock = application.Services.GetService<IHttpBankClientWrapper>() as HttpBankClientWrapperMock;
		}

		[Fact]
		public async Task GetExchangesForValidCurrency()
		{
			this._bankClientMock.SetMockResponse(HttpStatusCode.OK, "{\r\n\"rates\": [\r\n{\r\n \"currencyCode\": \"EUR\",\r\n\"amount\": 1,\r\n\"validFor\": \"2024-05-02\",\r\n\"rate\": 25.09\r\n}\r\n]\r\n}");

			HttpResponseMessage response = await _client.GetAsync("/v1/ExchangeRates?TargetCurrency=EUR");
			Assert.True(response.IsSuccessStatusCode);

			IEnumerable<ExchangeRate> body = await response.Content.ReadFromJsonAsync<IEnumerable<ExchangeRate>>();

			Assert.NotNull(body);
			Assert.NotEmpty(body);

			ExchangeRate rate = body.FirstOrDefault();
			Assert.NotNull(rate);
			Assert.Equal("CZK", rate.SourceCurrency.Code);
			Assert.Equal("EUR", rate.TargetCurrency.Code);
			Assert.Equal(DateTime.Parse("2024-05-02"), rate.ValidFor);
			Assert.Equal(25.09m, rate.Value);
		}

		[Fact]
		public async Task GetExchangesForValidCurrencyAndDate()
		{
			this._bankClientMock.SetMockResponse(HttpStatusCode.OK, "{\r\n\"rates\": [\r\n{\r\n\"currencyCode\": \"EUR\",\r\n\"amount\": 1,\r\n\"validFor\": \"2024-03-01\",\r\n\"rate\": 25.33\r\n},\r\n{\r\n\"currencyCode\": \"EUR\",\r\n\"amount\": 1,\r\n\"validFor\": \"2024-03-04\",\r\n\"rate\": 25.355\r\n},\r\n{\r\n\"currencyCode\": \"EUR\",\r\n\"amount\": 1,\r\n\"validFor\": \"2024-03-05\",\r\n\"rate\": 25.355\r\n},\r\n{\r\n\"currencyCode\": \"EUR\",\r\n\"amount\": 1,\r\n\"validFor\": \"2024-03-06\",\r\n\"rate\": 25.36\r\n},\r\n{\r\n\"currencyCode\": \"EUR\",\r\n\"amount\": 1,\r\n\"validFor\": \"2024-03-07\",\r\n\"rate\": 25.36\r\n},\r\n{\r\n\"currencyCode\": \"EUR\",\r\n\"amount\": 1,\r\n\"validFor\": \"2024-03-08\",\r\n\"rate\": 25.305\r\n},\r\n{\r\n\"currencyCode\": \"EUR\",\r\n\"amount\": 1,\r\n\"validFor\": \"2024-03-11\",\r\n\"rate\": 25.325\r\n},\r\n{\r\n\"currencyCode\": \"EUR\",\r\n\"amount\": 1,\r\n\"validFor\": \"2024-03-12\",\r\n\"rate\": 25.27\r\n},\r\n{\r\n\"currencyCode\": \"EUR\",\r\n\"amount\": 1,\r\n\"validFor\": \"2024-03-13\",\r\n\"rate\": 25.27\r\n},\r\n{\r\n\"currencyCode\": \"EUR\",\r\n\"amount\": 1,\r\n\"validFor\": \"2024-03-14\",\r\n\"rate\": 25.18\r\n},\r\n{\r\n\"currencyCode\": \"EUR\",\r\n\"amount\": 1,\r\n\"validFor\": \"2024-03-15\",\r\n\"rate\": 25.155\r\n},\r\n{\r\n\"currencyCode\": \"EUR\",\r\n\"amount\": 1,\r\n\"validFor\": \"2024-03-18\",\r\n\"rate\": 25.2\r\n},\r\n{\r\n\"currencyCode\": \"EUR\",\r\n\"amount\": 1,\r\n\"validFor\": \"2024-03-19\",\r\n\"rate\": 25.265\r\n},\r\n{\r\n\"currencyCode\": \"EUR\",\r\n\"amount\": 1,\r\n\"validFor\": \"2024-03-20\",\r\n\"rate\": 25.285\r\n},\r\n{\r\n\"currencyCode\": \"EUR\",\r\n\"amount\": 1,\r\n\"validFor\": \"2024-03-21\",\r\n\"rate\": 25.25\r\n},\r\n{\r\n\"currencyCode\": \"EUR\",\r\n\"amount\": 1,\r\n\"validFor\": \"2024-03-22\",\r\n\"rate\": 25.37\r\n},\r\n{\r\n\"currencyCode\": \"EUR\",\r\n\"amount\": 1,\r\n\"validFor\": \"2024-03-25\",\r\n\"rate\": 25.265\r\n},\r\n{\r\n\"currencyCode\": \"EUR\",\r\n\"amount\": 1,\r\n\"validFor\": \"2024-03-26\",\r\n\"rate\": 25.275\r\n},\r\n{\r\n\"currencyCode\": \"EUR\",\r\n\"amount\": 1,\r\n\"validFor\": \"2024-03-27\",\r\n\"rate\": 25.32\r\n},\r\n{\r\n\"currencyCode\": \"EUR\",\r\n\"amount\": 1,\r\n\"validFor\": \"2024-03-28\",\r\n\"rate\": 25.305\r\n}\r\n]\r\n}");

			DateTime date = DateTime.Parse("2024-03");
			HttpResponseMessage response = await _client.GetAsync($"/v1/ExchangeRates?TargetCurrency=EUR&Date={date}");

			Assert.True(response.IsSuccessStatusCode);

			IEnumerable<ExchangeRate> body = await response.Content.ReadFromJsonAsync<IEnumerable<ExchangeRate>>();

			Assert.NotNull(body);
			Assert.NotEmpty(body);
			Assert.Equal(20, body.Count());
		}

		[Theory]
		[InlineData("")]
		[InlineData("A")]
		[InlineData("AB")]
		[InlineData("ABC")]
		[InlineData("ABCD")]
		public async Task InvalidCurrency(string currency)
		{
			var response = await _client.GetAsync($"/v1/ExchangeRates?TargetCurrency={currency}");

			Assert.True(!response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public async Task InvalidResponseFromBankClient()
		{
			this._bankClientMock.SetMockResponse(HttpStatusCode.InternalServerError, string.Empty);

			var response = await _client.GetAsync($"/v1/ExchangeRates?TargetCurrency=EUR");

			Assert.True(!response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.BadGateway, response.StatusCode);
		}
	}
}
