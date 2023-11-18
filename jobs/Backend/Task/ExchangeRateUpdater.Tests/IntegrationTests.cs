using Bogus;
using ExchangeRateUpdater.Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Text.Json;

namespace ExchangeRateUpdater.Tests
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Faker _faker;

        private const string _endpoint = "api/v1/exchange_rates/daily/cnb";

        public IntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _faker = new Faker();
        }

        [Theory]
        [MemberData(nameof(GetCurrencies))]
        public async Task GetCnbExchangeRate_VariousCurrencies_Success(string code1, string code2)
        {
            // Arrange
            var request = new DailyExchangeRatesRequest()
            {
                CurrencyCodes = new List<string>() { code1, code2 }
            };
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync(_endpoint, CreateRequestBody(request));

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var payload = await ReadResponseBody<List<ExchangeRate>>(response);

            payload.Select(e => e.TargetCurrency.Code).Should().IntersectWith(request.CurrencyCodes);
        }

        [Fact]
        public async Task GetCnbExchangeRate_NoMatchingCurrencies_Success_EmptyResponse()
        {
            // Arrange
            var request = new DailyExchangeRatesRequest()
            {
                CurrencyCodes = new List<string>() { "AAA", "ZZZ" }
            };
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync(_endpoint, CreateRequestBody(request));

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var payload = await ReadResponseBody<List<ExchangeRate>>(response);

            payload.Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        public async Task GetCnbExchangeRate_InvalidCurrencyCode_Fails_WithValidationMessage()
        {
            // Arrange
            var request = new DailyExchangeRatesRequest()
            {
                CurrencyCodes = new List<string>()
                {
                    _faker.Random.String(100)
                }
            };

            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync(_endpoint, CreateRequestBody(request));

            // Assert
            response.IsSuccessStatusCode.Should().BeFalse();

            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().ContainAll("validation", "length");
        }

        private static IEnumerable<object[]> GetCurrencies()
        {
            yield return new object[] { "USD", "EUR" };
            yield return new object[] { "ZZZ", "USD" };
        }

        private static HttpContent CreateRequestBody<T>(T data) where T : class, new()
        {
            var json = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            return stringContent;
        }

        private static async Task<T> ReadResponseBody<T>(HttpResponseMessage response) where T : class, new()
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            T payload = JsonSerializer.Deserialize<T>(responseContent) ?? new T();
            return payload;
        }
    }
}