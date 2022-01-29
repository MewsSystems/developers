using ExchangeRateUpdater.ExternalServices.CzechNationalBank.Configuration;
using ExchangeRateUpdater.ExternalServices.CzechNationalBank.HttpClient;
using ExchangeRateUpdater.Providers.ExchangeRateProvider;
using Moq;

namespace ExchangeRateUpdater.Tests
{
    public class MockDomain
    {
        public Mock<ICzechNationalBankApiConfigurationProvider> CzechNationalBankApiConfigurationProvider => new();
        public Mock<ICzechNationalBankApiClient> CzechNationalBankApiClient => new();
        public Mock<IExchangeRateProvider> ExchangeRateProvider => new();

        public MockDomain()
        {
        }
    }
}

