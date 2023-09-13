using ExchangeRateUpdater.Infrastructure.Clients;
using ExchangeRateUpdater.Infrastructure.Models.CzechNationalBank;
using ExchangeRateUpdater.Infrastructure.Providers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.Extensions;
using RestSharp;
using Xunit;

namespace ExchangeRateUpdater.Infrastructure.UnitTests.Clients
{
    public class CzechNationalBankApiClientTests
    {
        private readonly IMonitorProvider _monitorProvider = Substitute.For<IMonitorProvider>();
        private readonly ILogger<CzechNationalBankApiClient> _logger = new NullLogger<CzechNationalBankApiClient>();
        private readonly CzechNationalBankApiClient _subjectUnderTest;

        public CzechNationalBankApiClientTests()
        {
            _subjectUnderTest = Substitute.ForPartsOf<CzechNationalBankApiClient>(Substitute.For<HttpClient>(), _monitorProvider, _logger);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ShouldCalExecuteGetAsync_WithCorrectParameters()
        {
            // Arrange
            var restRequest = new RestRequest();

            _subjectUnderTest.Configure().ExecuteGetAsync<CzechNationalBankExchangeRatesResponse>(Arg.Any<RestRequest>(), Arg.Any<string>())
                .Returns(new RestResponse<CzechNationalBankExchangeRatesResponse>(restRequest)
                {
                    IsSuccessStatusCode = true
                });

            // Act
            await _subjectUnderTest.GetExchangeRatesAsync();

            // Assert
            await _subjectUnderTest.Received(1).ExecuteGetAsync<CzechNationalBankExchangeRatesResponse>(
                Arg.Is<RestRequest>(r => r.Resource.Equals("cnbapi/exrates/daily")), 
                Arg.Any<string>());
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ForSuccessfulStatusCode_ShouldReturnCzechNationalBankExchangeRatesResponse()
        {
            // Arrange
            var restRequest = new RestRequest();
            var expectedResponse = new RestResponse<CzechNationalBankExchangeRatesResponse>(restRequest)
                {
                    IsSuccessStatusCode = true,
                    Data = new CzechNationalBankExchangeRatesResponse()
                };

            _subjectUnderTest.Configure().ExecuteGetAsync<CzechNationalBankExchangeRatesResponse>(Arg.Any<RestRequest>(), Arg.Any<string>())
                .Returns(expectedResponse);

            // Act
            var result = await _subjectUnderTest.GetExchangeRatesAsync();

            // Assert
            Assert.IsType<CzechNationalBankExchangeRatesResponse>(result);
            Assert.Equal(expectedResponse.Data, result);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ForNotSuccessfulStatusCode_ShouldThrowApplicationException()
        {
            // Arrange
            var restRequest = new RestRequest();
            var expectedResponse = new RestResponse<CzechNationalBankExchangeRatesResponse>(restRequest)
                {
                    IsSuccessStatusCode = false,
                    Data = new CzechNationalBankExchangeRatesResponse()
                };

            _subjectUnderTest.Configure().ExecuteGetAsync<CzechNationalBankExchangeRatesResponse>(Arg.Any<RestRequest>(), Arg.Any<string>())
                .Returns(expectedResponse);

            // Act
            // Assert
            var result = await Assert.ThrowsAsync<ApplicationException>(async () => await _subjectUnderTest.GetExchangeRatesAsync());
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ForNullDataResponse_ReturnsNull()
        {
            // Arrange
            var restRequest = new RestRequest();
            var expectedResponse = new RestResponse<CzechNationalBankExchangeRatesResponse>(restRequest)
                {
                    IsSuccessStatusCode = true
                };

            _subjectUnderTest.Configure().ExecuteGetAsync<CzechNationalBankExchangeRatesResponse>(Arg.Any<RestRequest>(), Arg.Any<string>())
                .Returns(expectedResponse);

            // Act
            var result = await _subjectUnderTest.GetExchangeRatesAsync();

            // Assert
            Assert.Null(result);
        }
    }
}
