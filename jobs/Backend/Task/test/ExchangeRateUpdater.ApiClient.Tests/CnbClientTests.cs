using ExchangeRateUpdater.ApiClient.Client;
using Moq;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.ApiClient.Client.ExchangeDaily;
using Newtonsoft.Json;
using System.Net;
using FluentAssertions;
using ExchangeRateUpdater.ApiClient.Exceptions;

namespace ExchangeRateUpdater.ApiClient.Tests
{
    public class CnbClientTests
    {
        private readonly Mock<ILogger<CnbClient>> _logger;
        private Mock<HttpClient> _httpClient;
        private readonly ICnbClient _sut;

        public CnbClientTests()
        {
            _logger = new Mock<ILogger<CnbClient>>();
            _httpClient = new Mock<HttpClient>();

            _sut = new CnbClient(_logger.Object, _httpClient.Object);
        }

        public void Ctor_When_AnyArgumentIsNull_Then_Throws_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => new CnbClient(default, default));
            Assert.Throws<ArgumentNullException>(() => new CnbClient(_logger.Object, default));

        }

        [Theory]
        [InlineData("2023-07-24", Language.EN)]
        public async Task When_Do_Request_Verify_Expected_Values_With_OK_Parmeters(
            string formatedDate,
            Language language)
        {
            var date = DateTime.Parse(formatedDate);
            var expected = GetExchangeRequest();

            SetupExchangesDaily();

            var actual = await _sut.GetExchangesDaily(date, language);

            Assert.NotNull(actual);
            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
            Assert.True(actual.IsSuccess);
            Assert.Single(actual.Payload.Rates);
            actual.Payload.Rates.FirstOrDefault().Should()
                .BeEquivalentTo(expected.Rates.FirstOrDefault());

            _httpClient.VerifyAll();
        }

        [Theory]
        [InlineData("2023-07-24", Language.EN)]
        public async Task When_Do_Request_BadRequest_Expected(
            string formatedDate,
            Language language)
        {
            var date = DateTime.Parse(formatedDate);
            var expected = GetErrorRequest();

            SetupBadRequest();

            var actual = await _sut.GetExchangesDaily(date, language);

            Assert.NotNull(actual);
            Assert.False(actual.IsSuccess);
            actual.Error.Should().BeEquivalentTo(expected);

            _httpClient.VerifyAll();
        }

        [Theory]
        [InlineData("2023-07-24", Language.EN)]
        public async Task When_Do_Request_NotFound_Expected(
            string formatedDate,
            Language language)
        {
            var date = DateTime.Parse(formatedDate);
            var expected = GetErrorRequest();

            var responseMEssage = new HttpResponseMessage(HttpStatusCode.NotFound);
            _httpClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .Throws<NullReferenceException>();

            await Assert.ThrowsAsync<ApiCnbException>(async () => await _sut.GetExchangesDaily(date, language));
        }

        private void SetupExchangesDaily()
        {
            var contentmock = new Mock<HttpContent>();
            var content = new StringContent(GetMockContent());
            var responseMEssage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = content
            };
            _httpClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(responseMEssage).Verifiable();
        }

        private void SetupBadRequest()
        {
            var contentmock = new Mock<HttpContent>();
            var content = new StringContent(GetErrorMockContent());
            var responseMEssage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = content
            };
            _httpClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(responseMEssage).Verifiable();
        }

        private string GetMockContent()
        {
            var result = GetExchangeRequest();
            return JsonConvert.SerializeObject(result);
        }

        private string GetErrorMockContent()
        {
            var result = GetErrorRequest();
            return JsonConvert.SerializeObject(result);
        }

        private ErrorResponse GetErrorRequest()
        {
            return new ErrorResponse()
            {
                Description = "Error",
                EndPoint = "error",
                ErrorCode = HttpStatusCode.BadRequest.ToString(),
                HappenedAt = DateTime.Parse("2023-07-24"),
                MessageId = "error"
            };
        }

        private ExchangeResponse GetExchangeRequest()
        {
            return new ExchangeResponse()
            {

                Rates = new List<ExchangeRateResponse>()
                {
                    new ExchangeRateResponse()
                    {
                        Amount = 1,
                        Country = "AUS",
                        Currency = "TEST",
                        CurrencyCode = "AUS",
                        Rate = 14,
                        Order = 10,
                        ValidFor = DateTime.Parse("2023-07-24")}
                }
            };
        }
    }
}