using ExchangeRateUpdater.Application.Banks;
using ExchangeRateUpdater.Domain.Enums;
using ExchangeRateUpdater.Infrastructure.Factories;

using FluentAssertions;

using Moq;

namespace ExchangeRateUpdater.UnitTests.Infrastructure.Factories
{
    public class BankFactoryTests
    {
        [Fact]
        public void Create_ValidBankIdentifier_ReturnsBankConnector()
        {
            // Arrange
            var bankId = BankIdentifier.CzechNationalBank;
            var mockBankConnector = new Mock<IBankConnector>();
            mockBankConnector.Setup(bc => bc.BankIdentifier).Returns(bankId);

            var mockBankConnectors = new List<IBankConnector> { mockBankConnector.Object };
            var bankFactory = new BankFactory(mockBankConnectors);

            // Act
            var result = bankFactory.Create(bankId);

            // Assert
            result.Should().Be(mockBankConnector.Object);
        }

        [Fact]
        public void Create_InvalidBankIdentifier_ThrowsException()
        {
            // Arrange
            var bankId = 999999;
            var mockBankConnectors = new List<IBankConnector>();
            var bankFactory = new BankFactory(mockBankConnectors);

            // Act
            Action act = () => bankFactory.Create((BankIdentifier)bankId);

            // Assert
            act.Should().Throw<NotSupportedException>().WithMessage($"Bank '{bankId}' is not supported.");
        }
    }
}
