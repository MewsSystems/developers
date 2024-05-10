using AutoFixture;
using ExchangeRateUpdater.Core.Configuration;
using ExchangeRateUpdater.CzechNationalBank.Api;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.CzechNationalBankApiTests
{
    public class CzechNationalBankExchangeRateProviderTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly CzechNationalBankExchangeRateProvider _sut;
        private readonly Mock<ICzechNationalBankApi> _czechNationalBankApiMock;
        private readonly Mock<ILogger<CzechNationalBankExchangeRateProvider>> _logger;

        public CzechNationalBankExchangeRateProviderTests()
        {
            var configuration = _fixture.Create<CzechNationalBankConfiguration>();
            _czechNationalBankApiMock = new Mock<ICzechNationalBankApi>(MockBehavior.Strict);
            _logger = new Mock<ILogger<CzechNationalBankExchangeRateProvider>>(MockBehavior.Strict);

            _sut = new CzechNationalBankExchangeRateProvider(_czechNationalBankApiMock.Object, configuration, _logger.Object);
        }
    }
}