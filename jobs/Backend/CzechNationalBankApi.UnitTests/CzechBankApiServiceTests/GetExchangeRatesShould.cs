using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

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
            _mockHttpMessageHandler.MockSend(Arg.Is<HttpRequestMessage>(x => x.RequestUri!.OriginalString.Contains("daily.txt")), Arg.Any<CancellationToken>())
               .Returns(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(string.Empty) });

            _mockHttpMessageHandler.MockSend(Arg.Is<HttpRequestMessage>(x => x.RequestUri!.OriginalString.Contains("fx_rates.txt")), Arg.Any<CancellationToken>())
                .Returns(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(string.Empty) });

            var actual = await _czechBankApiService.GetExchangeRatesAsync();

            actual.Should().BeEmpty();
        }

        [Fact]
        public async Task ReturnExpectedData_MapsCorrectlyFromApiTextFormat()
        {
            var dummyApiDailyResponseData = @$"11 Jul 2024 #133{Environment.NewLine}Country|Currency|Amount|Code|Rate{Environment.NewLine}Australia|dollar|1|AUD|15.777";

            var dummyApiFxRatesResponseData = @$"28 Jun 2024 #6{Environment.NewLine}Country|Currency|Amount|Code|Rate{Environment.NewLine}Afghanistan|afghani|100|AFN|32.957{Environment.NewLine}Albania|lek|100|ALL|25.052";

            _mockHttpMessageHandler.MockSend(Arg.Is<HttpRequestMessage>(x => x.RequestUri!.OriginalString.Contains("daily.txt")), Arg.Any<CancellationToken>())
                .Returns(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(dummyApiDailyResponseData) });

            _mockHttpMessageHandler.MockSend(Arg.Is<HttpRequestMessage>(x => x.RequestUri!.OriginalString.Contains("fx_rates.txt")), Arg.Any<CancellationToken>())
                .Returns(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(dummyApiFxRatesResponseData) });

            var expected = new List<CzechExchangeItemDto>() { 
                new CzechExchangeItemDto { Country = "Australia", Currency = "dollar", Amount = 1, Code = "AUD", Rate = 15.777m },
                new CzechExchangeItemDto { Country = "Afghanistan", Currency = "afghani", Amount = 100, Code = "AFN", Rate = 32.957m },
                new CzechExchangeItemDto { Country = "Albania", Currency = "lek", Amount = 100, Code = "ALL", Rate = 25.052m },
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
            var dummyApiDailyResponseData = @$"11 Jul 2024 #133{Environment.NewLine}Country|Currency|Amount|Code|Rate{Environment.NewLine}Australia|dollar|1|AUD|15.777";

            var dummyApiFxRatesResponseData = @$"28 Jun 2024 #6{Environment.NewLine}Country|Currency|Amount|Code|Rate{Environment.NewLine}Afghanistan|afghani|100|AFN|32.957{Environment.NewLine}Albania|lek|100|ALL|25.052{Environment.NewLine}Australia|dollar|1|AUD|15.777";

            _mockHttpMessageHandler.MockSend(Arg.Is<HttpRequestMessage>(x => x.RequestUri!.OriginalString.Contains("daily.txt")), Arg.Any<CancellationToken>())
                .Returns(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(dummyApiDailyResponseData) });

            _mockHttpMessageHandler.MockSend(Arg.Is<HttpRequestMessage>(x => x.RequestUri!.OriginalString.Contains("fx_rates.txt")), Arg.Any<CancellationToken>())
                .Returns(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(dummyApiFxRatesResponseData) });

            var actual = _czechBankApiService.GetExchangeRatesAsync;

            await actual.Should().ThrowAsync<Exception>().WithMessage("Duplicate currency code information found");
        }
    }
}