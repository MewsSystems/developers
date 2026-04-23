using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdate.Testing;
using ExchangeRateUpdater.Core.Adapters.CzechNationalBankApi;
using ExchangeRateUpdater.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Moq.AutoMock;
using Moq.Protected;
using NUnit.Framework;

namespace ExchangeRateUpdater.Core.UnitTests.Adapters.CzechNationalBankApi;

internal partial class CzechNationalBankApiAdapterTests
{
	public class TestContext : TestContext<CzechNationalBankApiAdapter>
	{
		private readonly string _successResponse = @"{
  ""rates"": [
    {
      ""validFor"": ""2023-10-25"",
      ""order"": 206,
      ""country"": ""Australia"",
      ""currency"": ""dollar"",
      ""amount"": 1,
      ""currencyCode"": ""AUD"",
      ""rate"": 14.81
    }]}";

		protected override CzechNationalBankApiAdapter BuildSut(AutoMocker autoMocker)
		{
			var testHttpClient = new HttpClient(autoMocker.Get<HttpMessageHandler>())
			{
				BaseAddress = new Uri("http://testing.com/")
			};

			return new CzechNationalBankApiAdapter(testHttpClient, AutoMocker.GetMock<IMemoryCache>().Object);
		}

		public TestContext WithHttpResponse(HttpStatusCode code, bool emptyResponse = false)
		{
			var messageHandler = AutoMocker.GetMock<HttpMessageHandler>(true);
			var response = new HttpResponseMessage
			{
				StatusCode = code,
				Content = new StringContent(emptyResponse ? "" : _successResponse)
			};

			messageHandler
				.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response);

			return this;
		}

		public void AssertSuccessResponse(ExchangeRate[] exchangeRates, bool emptyResponse)
		{
			if (emptyResponse)
			{
				Assert.That(exchangeRates, Is.Empty);
			}
			else
			{
				Assert.That(exchangeRates, Is.Not.Empty);
				Assert.That(exchangeRates[0].SourceCurrency.Code, Is.EqualTo("AUD"));
				Assert.That(exchangeRates[0].TargetCurrency.Code, Is.EqualTo("CZK"));
			}
		}
	}
}