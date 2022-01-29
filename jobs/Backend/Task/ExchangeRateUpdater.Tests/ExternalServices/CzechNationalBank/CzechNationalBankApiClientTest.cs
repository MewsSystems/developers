using System;
using System.Threading.Tasks;
using ExchangeRateUpdater.ExternalServices.CzechNationalBank.Configuration;
using ExchangeRateUpdater.ExternalServices.CzechNationalBank.HttpClient;
using Moq;
using NUnit.Framework;

namespace ExchangeRateUpdater.Tests.ExternalServices
{
    [TestFixture]
    public class CzechNationalBankApiClientTest : TestBase
    { 
        CzechNationalBankApiClient _apiClient;
    
        [SetUp]
        public void SetUp()
        {
            _apiClient = new CzechNationalBankApiClient(Mock.CzechNationalBankApiConfigurationProvider.Object);
        }

        [Test]
        public async Task GetExchangeRatesAsync_ShouldReturnRates()
        {
            // Given
            var configuration = new CzechNationalBankConfiguration
            {
                HttpProtocol = "http://",
                DomainUrl = "domain",
                Endpoints = new()
                {
                    ExchangeRatePath = "path"
                }
            };
            Mock.CzechNationalBankApiConfigurationProvider
                .Setup(p => p.GetConfiguration())
                .Returns(configuration);
            var responseString = 
                "27 Jan 2022 #19/n" +
                "Country|Currency|Amount|Code|Rate/n" +
                "Australia|dollar|1|AUD|15.492" +
                "Brazil|real|1|BRL|4.061";
            /*var resposne = new HttpResponseMessage
            {
                Content = new StringContent(responseString)
            };
            
    
            var resposne = "";*/
        
            // When
            var response = await _apiClient.GetExchangeRatesAsync(DateTime.UtcNow);

            // Then
            Mock.CzechNationalBankApiConfigurationProvider
                .Verify(p => p.GetConfiguration().Equals(configuration), Times.Once);
        }
    }
}

