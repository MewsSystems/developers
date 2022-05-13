using Core.Infra.Interfaces;
using Core.Infra.Mappers;
using Core.Infra.Repositories;
using FluentAssertions;
using Moq;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Infra.Repositories
{
    public class ExchangeRateRepositoryTests
    {
        public ExchangeRateRepositoryTests()
        {
            MockRepository = new MockRepository(MockBehavior.Strict);
            MockExchangeRateClient = MockRepository.Create<IExchangeRateClient>();
        }

        private MockRepository MockRepository { get; set; }

        private Mock<IExchangeRateClient> MockExchangeRateClient { get; set; }

        private ExchangeRateRepository ExchangeRateRepository => new ExchangeRateRepository(
            MockExchangeRateClient.Object, new ExchangeRateDtoMapper(), new ExchangeRateMapper());

        [Fact(DisplayName = "ERR-001: GetExchangeRate should return all exchange rates.")]
        public async Task FR001()
        {
            // Arrange
            var source = File.ReadAllText("Content/exchangeRateContent_Successful.txt"); ;
            MockExchangeRateClient.Setup(x => x.GetExchangeRates()).ReturnsAsync(source);

            // Act
            var result = await ExchangeRateRepository.GetExchangeRates();

            // Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}
