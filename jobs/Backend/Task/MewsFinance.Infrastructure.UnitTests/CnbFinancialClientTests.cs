using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using MewsFinance.Domain.Models;
using MewsFinance.Infrastructure.CnbFinancialClient;
using MewsFinance.Infrastructure.CnbFinancialClient.Mappings;
using MewsFinance.Infrastructure.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System.Net;
using Xunit;

namespace MewsFinance.Infrastructure.UnitTests
{
    public class CnbFinancialClientTests
    {
        private readonly CnbFinancialClient.CnbFinancialClient _sut;
        private readonly IMapper _mapper;
        private readonly Mock<IHttpClientWrapper> _httpClientWrapperMock;

        public CnbFinancialClientTests()
        {
            _httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            _mapper = SetupMapper();
            var loggerMock = new Mock<ILogger<CnbFinancialClient.CnbFinancialClient>>();

            _sut = new CnbFinancialClient.CnbFinancialClient(_httpClientWrapperMock.Object, _mapper, loggerMock.Object);
        }

        private static IMapper SetupMapper()
        {
            var config = new MapperConfiguration(cfg =>
               cfg.AddProfile<CnbFinancialClientProfile>());
            var mapper = new Mapper(config);

            return mapper;
        }

        [Theory, AutoData]
        public async Task When_Calling_Api_And_Receiving_Successful_Response_Then_Return_Exchange_Rates
            (CnbExchangeRateResponse cnbExchangeRateResponse)
        {
            // Arrange
            var expectedExchangeRates = MapToExchangeRates(
                cnbExchangeRateResponse.Rates,
                _sut.TargetCurrencyCode);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(cnbExchangeRateResponse))
            };            
            _httpClientWrapperMock.Setup(w => w.GetAsync(It.IsAny<string>()))
                                  .ReturnsAsync(httpResponseMessage);

            // Act
            var exchangeRateResponse = await _sut.GetExchangeRates(DateTime.UtcNow);

            // Assert
            exchangeRateResponse.Should().NotBeNull();
            exchangeRateResponse.IsSuccess.Should().BeTrue();
            exchangeRateResponse.Data.Should().BeEquivalentTo(expectedExchangeRates);
            exchangeRateResponse.Message.Should().BeEmpty();
        }

        [Fact]
        public async Task When_Calling_Api_And_Receiving_Exception_Then_Return_No_Exchange_Rates_And_Error_Flag()
        {
            // Arrange
            _httpClientWrapperMock.Setup(w => w.GetAsync(It.IsAny<string>()))
                                  .ThrowsAsync(new Exception());
            // Act
            var exchangeRateResponse = await _sut.GetExchangeRates(DateTime.UtcNow);

            // Assert
            exchangeRateResponse.Should().NotBeNull();
            exchangeRateResponse.IsSuccess.Should().BeFalse();
            exchangeRateResponse.Data.Should().BeEmpty();
            exchangeRateResponse.Message.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.NotFound)]
        public async Task When_Calling_Api_And_Receiving_Http_Error_Then_Return_No_Exchange_Rates_And_Error_Flag
            (HttpStatusCode httpStatusCode)
        {
            // Arrange
            _httpClientWrapperMock.Setup(w => w.GetAsync(It.IsAny<string>()))
                                  .ReturnsAsync(new HttpResponseMessage(httpStatusCode));
            // Act
            var exchangeRateResponse = await _sut.GetExchangeRates(DateTime.UtcNow);

            // Assert
            exchangeRateResponse.Should().NotBeNull();
            exchangeRateResponse.IsSuccess.Should().BeFalse();
            exchangeRateResponse.Data.Should().BeEmpty();
            exchangeRateResponse.Message.Should().NotBeEmpty();
        }

        private IEnumerable<ExchangeRate> MapToExchangeRates(
            IEnumerable<CnbExchangeRate> cnbExchangeRates,
            string targetCurrencyCode)
        {
            var exchangeRates = _mapper.Map<IEnumerable<ExchangeRate>>(
                cnbExchangeRates,
                opt => opt.Items[MappingConstants.TargetCurrencyCode] = targetCurrencyCode);

            return exchangeRates;
        }
    }
}
