using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Net.Http.Json;

namespace CzechNationalBankApi.UnitTests.CzechBankApiServiceTests
{
    public class GetExchangeRatesShould
    {
        private MockHttpMessageHandler _mockHttpMessageHandler = Substitute.ForPartsOf<MockHttpMessageHandler>();
        private HttpClient _httpClient;
        private ILogger<CzechBankApiService> _logger = Substitute.For<ILogger<CzechBankApiService>>();
        private CzechBankApiService _czechBankApiService;

        public GetExchangeRatesShould()
        {
            _httpClient = new HttpClient(_mockHttpMessageHandler) { BaseAddress = new Uri("https://localhost/") };

            _czechBankApiService = new CzechBankApiService(_logger, _httpClient);
        }

        [Fact]
        public async Task ReturnsEmpty_WhenApiReturnsNoData()
        {
            _mockHttpMessageHandler.MockSend(Arg.Is<HttpRequestMessage>(x => x.RequestUri!.OriginalString.Contains("cnbapi/exrates/daily")), Arg.Any<CancellationToken>())
               .Returns(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = JsonContent.Create(new CzechExchangeRatesResponseDto()) });

            _mockHttpMessageHandler.MockSend(Arg.Is<HttpRequestMessage>(x => x.RequestUri!.OriginalString.Contains("cnbapi/fxrates/daily-month")), Arg.Any<CancellationToken>())
                .Returns(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = JsonContent.Create(new CzechExchangeRatesResponseDto()) });

            var actual = await _czechBankApiService.GetExchangeRatesAsync();

            actual.Should().BeEmpty();
        }

        [Fact]
        public async Task ReturnExpectedData_MapsCorrectlyFromApiTextFormat()
        {
            var dummyApiExRatesDailyResponseData = new CzechExchangeRatesResponseDto
            {
                Rates = new List<CzechExchangeItemDto> {
                    new CzechExchangeItemDto { Country = "Australia", Currency = "dollar", Amount = 1, CurrencyCode = "AUD", Rate = 15.763m }
                }
            };

            var dummyApiFxRatesResponseData = new CzechExchangeRatesResponseDto
            {
                Rates = new List<CzechExchangeItemDto> {
                    new CzechExchangeItemDto { Country = "Afghanistan", Currency = "afghani", Amount = 100, CurrencyCode = "AFN", Rate = 32.957m },
                    new CzechExchangeItemDto { Country = "Albania", Currency = "lek", Amount = 100, CurrencyCode = "ALL", Rate = 25.052m }
                }
            };

            _mockHttpMessageHandler.MockSend(Arg.Is<HttpRequestMessage>(x => x.RequestUri!.OriginalString.Contains("cnbapi/exrates/daily")), Arg.Any<CancellationToken>())
                .Returns(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = JsonContent.Create(dummyApiExRatesDailyResponseData) });

            _mockHttpMessageHandler.MockSend(Arg.Is<HttpRequestMessage>(x => x.RequestUri!.OriginalString.Contains("cnbapi/fxrates/daily-month")), Arg.Any<CancellationToken>())
                .Returns(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = JsonContent.Create(dummyApiFxRatesResponseData) });

            var expected = new List<CzechExchangeItemDto>() { 
                new CzechExchangeItemDto { Country = "Australia", Currency = "dollar", Amount = 1, CurrencyCode = "AUD", Rate = 15.763m },
                new CzechExchangeItemDto { Country = "Afghanistan", Currency = "afghani", Amount = 100, CurrencyCode = "AFN", Rate = 32.957m },
                new CzechExchangeItemDto { Country = "Albania", Currency = "lek", Amount = 100, CurrencyCode = "ALL", Rate = 25.052m },
            };

            var actual = await _czechBankApiService.GetExchangeRatesAsync();

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ThrowExceptionAndNotSwallow_WhenApiDataCannotBeParsedIntoType()
        {
            _mockHttpMessageHandler.MockSend(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>()).Throws(new Exception("dummy-exception"));

            var actual = async () => await _czechBankApiService.GetExchangeRatesAsync();

            await actual.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task ThrowsExceptionWhenDuplicateCurrencyInfoFound()
        {
            var dummyApiExRatesDailyResponseData = new CzechExchangeRatesResponseDto { 
                Rates = new List<CzechExchangeItemDto> {
                    new CzechExchangeItemDto { Country = "Australia", Currency = "dollar", Amount = 1, CurrencyCode = "AUD", Rate = 15.763m } 
                }
            };

            var dummyApiFxRatesResponseData = new CzechExchangeRatesResponseDto { 
                Rates = new List<CzechExchangeItemDto> {
                    new CzechExchangeItemDto { Country = "Afghanistan", Currency = "afghani", Amount = 100, CurrencyCode = "AFN", Rate = 32.957m },
                    new CzechExchangeItemDto { Country = "Albania", Currency = "lek", Amount = 100, CurrencyCode = "ALL", Rate = 25.052m },
                    new CzechExchangeItemDto { Country = "Australia", Currency = "dollar", Amount = 1, CurrencyCode = "AUD", Rate = 15.763m }
                }
            };

            _mockHttpMessageHandler.MockSend(Arg.Is<HttpRequestMessage>(x => x.RequestUri!.OriginalString.Contains("cnbapi/exrates/daily")), Arg.Any<CancellationToken>())
                .Returns(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = JsonContent.Create(dummyApiExRatesDailyResponseData) });

            _mockHttpMessageHandler.MockSend(Arg.Is<HttpRequestMessage>(x => x.RequestUri!.OriginalString.Contains("cnbapi/fxrates/daily-month")), Arg.Any<CancellationToken>())
                .Returns(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = JsonContent.Create(dummyApiFxRatesResponseData) });

            var actual = _czechBankApiService.GetExchangeRatesAsync;

            await actual.Should().ThrowAsync<Exception>().WithMessage("Duplicate currency code information found");
        }
    }
}