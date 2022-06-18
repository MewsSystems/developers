using AutoFixture;
using ExchangeRateUpdated.Service.Parsers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Tests
{
    [TestClass]
    public class CnbExchangeRateProviderShould
    {
        private Fixture _fixture = default!;
        private Mock<HttpMessageHandler> _messageHandler = default!;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture();
            _messageHandler = new Mock<HttpMessageHandler>();
        }

        [TestMethod]
        public async Task Returns_Ok_When_Response_Ok()
        {
            
            var httpClient = GenericRequestTestPrepration(CnbExchangeRateProvider.SourceUrl, HttpStatusCode.OK, string.Empty);
            var parser = new CnbCsvParser();

            var service = new CnbExchangeRateProvider(httpClient, parser);

            var result = await service.GetExchangeRatesAsync(new List<Currency> { new Currency("USD") });

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEmpty();
        }

        [TestMethod]
        [DataRow(HttpStatusCode.BadGateway)]
        [DataRow(HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.NotFound)]
        public async Task Returns_Failure_When_Response_Failure(HttpStatusCode httpStatusCode)
        {
            var httpClient = GenericRequestTestPrepration(CnbExchangeRateProvider.SourceUrl, httpStatusCode, string.Empty);
            var parser = new CnbCsvParser();
            var service = new CnbExchangeRateProvider(httpClient, parser);

            var result = await service.GetExchangeRatesAsync(new List<Currency> { new Currency("USD") });

            result.IsFailed.Should().BeTrue();
        }

        [TestMethod]
        public async Task Returns_Ok_And_Currencies_Within_The_List()
        {
            
            var sample = File.OpenText("./Samples/17June.txt").ReadToEnd();
            var httpClient = GenericRequestTestPrepration(CnbExchangeRateProvider.SourceUrl, HttpStatusCode.OK, sample);
            var parser = new CnbCsvParser();

            var service = new CnbExchangeRateProvider(httpClient, parser);

            var currencies = new List<Currency> { new Currency("USD"), new Currency("EUR") };
            var result = await service.GetExchangeRatesAsync(currencies);

            result.IsSuccess.Should().BeTrue();
            result.Value.Count().Should().Be(currencies.Count);

            foreach (var exchangeRate in result.Value)
            {
                exchangeRate.TargetCurrency.Should().Be(CnbExchangeRateProvider.DefaultCurrencyCode);
                exchangeRate.SourceCurrency.Code.Should().BeOneOf(currencies.Select(c => c.Code));
            }
        }

        [TestMethod]
        public async Task Returns_Ok_And_Ignores_Unpresent_Currencies_Within_Response()
        {
            
            var sample = File.OpenText("./Samples/17June.txt").ReadToEnd();
            var httpClient = GenericRequestTestPrepration(CnbExchangeRateProvider.SourceUrl, HttpStatusCode.OK, sample);
            var parser = new CnbCsvParser();

            var service = new CnbExchangeRateProvider(httpClient, parser);

            var currencies = new List<Currency> { new Currency("USD"), new Currency("EUR") };
            var myCurrency = "KLM";
            currencies.Add(myCurrency);

            var result = await service.GetExchangeRatesAsync(currencies);

            result.IsSuccess.Should().BeTrue();
            result.Value.Count().Should().Be(2); // KLM currency is ignored
        }

        [TestMethod]
        public async Task Returns_Failure_When_Parsing_Failures()
        {
            
            var sample = File.OpenText("./Samples/InvalidData.txt").ReadToEnd();
            var httpClient = GenericRequestTestPrepration(CnbExchangeRateProvider.SourceUrl, HttpStatusCode.OK, sample);
            var parser = new CnbCsvParser();

            var service = new CnbExchangeRateProvider(httpClient, parser);

            var result = await service.GetExchangeRatesAsync(new List<Currency> { new Currency("USD") });

            result.IsFailed.Should().BeTrue();
        }

        private HttpClient GenericRequestTestPrepration(string url, HttpStatusCode httpStatusCode,  string response)
        {
            _messageHandler
                .Protected()
                    .Setup<Task<HttpResponseMessage>>("SendAsync",
                        ItExpr.Is<HttpRequestMessage>(c => c.RequestUri.ToString().Contains(url)
                            && c.Method == HttpMethod.Get),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage()
                    {
                        StatusCode = httpStatusCode,
                        Content = new StringContent(response, Encoding.UTF8)
                    });

            var httpClient = new HttpClient(_messageHandler.Object);

            return httpClient;
        }
    }
}
