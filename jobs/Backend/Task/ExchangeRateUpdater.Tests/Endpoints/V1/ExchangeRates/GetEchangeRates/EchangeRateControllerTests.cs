using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using ExchangeRateUpdater.Host;
using System;
using System.Net.Http.Json;
using System.Collections.Generic;
using ExchangeRateUpdater.Domain.Core.UseCases.CommonModels;

namespace ExchangeRateUpdater.Tests.Endpoints.V1.ExchangeRates.GetEchangeRates
{
	public class EchangeRateControllerTests : IClassFixture<WebApplicationFactory<Program>>
	{
		readonly HttpClient _client;

		public EchangeRateControllerTests(WebApplicationFactory<Program> application)
		{
			_client = application.CreateClient();
		}

		[Fact]
		public async Task GetExchangesForValidCurrency()
		{
			HttpResponseMessage response = await _client.GetAsync("/v1/ExchangeRates?TargetCurrency=EUR");
			Assert.True(response.IsSuccessStatusCode);

			IEnumerable<ExchangeRate>? body = await response.Content.ReadFromJsonAsync<IEnumerable<ExchangeRate>>();
			
			Assert.NotNull(body);
			Assert.NotEmpty(body);
		}

		[Fact]
		public async Task GetExchangesForValidCurrencyAndDate()
		{
			DateTime date = DateTime.Now;
			HttpResponseMessage response = await _client.GetAsync($"/v1/ExchangeRates?TargetCurrency=EUR&Date={date}");
			
			Assert.True(response.IsSuccessStatusCode);
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
	}
}
