using Microsoft.Extensions.Logging;
using NSubstitute;

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
            _httpClient = new HttpClient(_mockHttpMessageHandler);

            _czechBankApiService = new CzechBankApiService(_logger, _httpClient);
        }

        [Fact]
        public async Task ReturnExpectedData()
        {

        }

        [Fact]
        public async Task ThrowExceptionAndNotSwallow()
        {

        }
    }
}