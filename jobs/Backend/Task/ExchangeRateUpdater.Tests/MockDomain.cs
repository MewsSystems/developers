using ExchangeRateUpdater.ExternalServices.CzechNationalBank.Configuration;
using ExchangeRateUpdater.ExternalServices.CzechNationalBank.HttpClient;
using Moq;

namespace ExchangeRateUpdater.Tests
{
    public class MockDomain
    {
        public Mock<ICzechNationalBankApiConfigurationProvider> CzechNationalBankApiConfigurationProvider = new();
        public Mock<ICzechNationalBankApiClient> CzechNationalBankApiClient = new();

        public MockDomain()
        {
        }
    }
}

