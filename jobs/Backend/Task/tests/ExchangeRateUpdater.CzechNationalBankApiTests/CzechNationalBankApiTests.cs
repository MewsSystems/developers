using AutoFixture;
using ExchangeRateUpdater.CzechNationalBank.Api;
using Moq;

namespace ExchangeRateUpdater.CzechNationalBankApiTests
{
    public class CzechNationalBankApiTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<IHttpClientFactory> _clientFactory;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly CzechNationalBankApi _sut;

        public CzechNationalBankApiTests()
        {
            _clientFactory = new Mock<IHttpClientFactory>();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            _sut = new CzechNationalBankApi(
                _clientFactory.Object
            );
        }
    }
}
