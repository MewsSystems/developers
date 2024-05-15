using Adpater.Http.CzechNationalBank;
using Adpater.Http.CzechNationalBank.Test.Fakes;
using ExchangeRateUpdater.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Adpater.Http.CzechNationalBank.Test
{
    public class CzechNationalBankExchangeRateProviderTest
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private FakeHttpMessageHandler _fakeHttpMessageHandler = new FakeHttpMessageHandler();

        public CzechNationalBankExchangeRateProviderTest()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var httpClient = new HttpClient(_fakeHttpMessageHandler);
            httpClient.BaseAddress = new Uri("http://example.com");

            _httpClientFactoryMock.Setup(x => x.CreateClient("CzechNationalBankApi")).Returns(httpClient);
        }

        [Fact]
        public void CzechNationalBankExchangeRateProvider_GetDailyExchangeRates_TargetCurrencyIsNotCZK_ShouldThrowAnArgumentException()
        {
            var sut = new CzechNationalBankExchangeRateProvider(_httpClientFactoryMock.Object, new NullLogger<CzechNationalBankExchangeRateProvider>());

            Action callingProvider = () => sut.GetDailyExchangeRates(Currency.Create("ABC"), default(CancellationToken)).Wait();

            callingProvider.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CzechNationalBankExchangeRateProvider_GetDailyExchangeRates_TargetCurrencyNull_ShouldThrowAnArgumentException()
        {
            var sut = new CzechNationalBankExchangeRateProvider(_httpClientFactoryMock.Object, new NullLogger<CzechNationalBankExchangeRateProvider>());

            Action callingProvider = () => sut.GetDailyExchangeRates(null, default(CancellationToken)).Wait();

            callingProvider.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CzechNationalBankExchangeRateProvider_GetDailyExchangeRates_HttpCodeIs400_ShouldThrowAnException()
        {
            _fakeHttpMessageHandler.SetReponse(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest
            });

            var sut = new CzechNationalBankExchangeRateProvider(_httpClientFactoryMock.Object, new NullLogger<CzechNationalBankExchangeRateProvider>());

            Action callingProvider = () => sut.GetDailyExchangeRates(Currency.Create("CZK"), default(CancellationToken)).Wait();

            callingProvider.Should().Throw<Exception>();
        }

        [Fact]
        public void CzechNationalBankExchangeRateProvider_GetDailyExchangeRates_HttpCodeIs500_ShouldThrowAnException()
        {
            _fakeHttpMessageHandler.SetReponse(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            });

            var sut = new CzechNationalBankExchangeRateProvider(_httpClientFactoryMock.Object, new NullLogger<CzechNationalBankExchangeRateProvider>());

            Action callingProvider = () => sut.GetDailyExchangeRates(Currency.Create("CZK"), default(CancellationToken)).Wait();

            callingProvider.Should().Throw<Exception>();
        }

        [Fact]
        public async Task CzechNationalBankExchangeRateProvider_GetDailyExchangeRates_HttpCodeIs200_AndAmountIsZero_ShouldSkipValue()
        {
            _fakeHttpMessageHandler.SetReponse(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(@"{
                  ""rates"": [
                    {
                      ""amount"": 0,
                      ""country"": ""Brazil"",
                      ""currency"": ""BRL"",
                      ""currencyCode"": ""BRL"",
                      ""order"": 0,
                      ""rate"": 4,
                      ""validFor"": ""2024-05-14""
                    }
                  ]
                }")
            });

            var sut = new CzechNationalBankExchangeRateProvider(_httpClientFactoryMock.Object, new NullLogger<CzechNationalBankExchangeRateProvider>());

            var exchangeRates = await sut.GetDailyExchangeRates(Currency.Create("CZK"), default(CancellationToken));

            exchangeRates.Should().BeEmpty();
        }

        [Fact]
        public async Task CzechNationalBankExchangeRateProvider_GetDailyExchangeRates_HttpCodeIs200_AndContentValid_ShouldReturnExchangeRateList()
        {
            _fakeHttpMessageHandler.SetReponse(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(@"{
                  ""rates"": [
                    {
                      ""amount"": 1,
                      ""country"": ""Brazil"",
                      ""currency"": ""BRL"",
                      ""currencyCode"": ""BRL"",
                      ""order"": 0,
                      ""rate"": 4,
                      ""validFor"": ""2024-05-14""
                    }
                  ]
                }")
            });

            var sut = new CzechNationalBankExchangeRateProvider(_httpClientFactoryMock.Object, new NullLogger<CzechNationalBankExchangeRateProvider>());

            var exchangeRates = await sut.GetDailyExchangeRates(Currency.Create("CZK"), default(CancellationToken));

            exchangeRates.Should().NotBeNullOrEmpty();
            var exchangeRate = exchangeRates.FirstOrDefault();

            exchangeRate.SourceCurrency.Should().BeEquivalentTo(Currency.Create("BRL"));
            exchangeRate.TargetCurrency.Should().BeEquivalentTo(Currency.Create("CZK"));
            exchangeRate.Value.Should().Be(4);
        }

        [Fact]
        public void CzechNationalBankExchangeRateProvider_GetDailyExchangeRates_HttpCodeIs200_AndContentInvalid_ShouldThrowAnException()
        {
            _fakeHttpMessageHandler.SetReponse(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(@"[]")
            }); ;

            var sut = new CzechNationalBankExchangeRateProvider(_httpClientFactoryMock.Object, new NullLogger<CzechNationalBankExchangeRateProvider>());

            Action callingProvider = () => sut.GetDailyExchangeRates(Currency.Create("CZK"), default(CancellationToken)).Wait();

            callingProvider.Should().Throw<Exception>();
        }
    }
}
